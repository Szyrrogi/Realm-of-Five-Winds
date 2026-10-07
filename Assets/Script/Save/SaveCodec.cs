using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

/// <summary>
/// Kompaktowy format składu do bazy i Photona.
///
///   "~2" + Base64Url( bajty )
///
/// Bajty:
///   [1]  wersja (=2)
///   str  playerName (varint długość + UTF-8)
///   int  liczba pól nagłówka N, potem N liczb (zigzag varint) – patrz HEADER_*
///   int  liczba jednostek, potem każda jednostka:
///        varint  (Pole << 3) | (Line + 1)
///        varint  Id
///        varint  maska pól (bit i = pole i jest zapisane, bo != 0)
///        zigzag varint dla każdego ustawionego bitu, w kolejności indeksów
///
/// Zera nie są zapisywane, a Health jest trzymane jako (MaxHealth - Health), czyli 0 przy pełnym HP.
/// Nowe pola dopisujemy NA KOŃCU tablic – stare zapisy dalej się wczytają (brakujące = 0),
/// a dekoder pomija bity, których nie zna (zapis z nowszej wersji gry też się wczyta).
/// </summary>
public static class SaveCodec
{
    public const string Prefix = "~2";
    const byte Version = 2;

    // ---- Nagłówek: indeksy w liście liczb nagłówka ----
    const int HEADER_NUMBER = 0, HEADER_BOHATER = 1, HEADER_LEVELUP = 2, HEADER_LINE0 = 3; // LINE0..LINE0+2
    const int HEADER_BOHATER_LEVEL = 6;

    // ---- Pola jednostki: KOLEJNOŚĆ JEST CZĘŚCIĄ FORMATU, dopisuj tylko na końcu ----
    const int F_HEALTH_MISSING = 4;
    static readonly Func<SaveManager.SaveUnit, int>[] Get =
    {
        u => u.Cost,                    // 0
        u => u.RealCost,                // 1
        u => u.Initiative,              // 2
        u => u.MaxHealth,               // 3
        u => u.MaxHealth - u.Health,    // 4  (brakujące HP)
        u => u.Attack,                  // 5
        u => u.Defense,                 // 6
        u => u.Range,                   // 7
        u => u.AP,                      // 8
        u => u.MagicResist,             // 9
        u => u.UpgradeLevel,            // 10
        u => u.SpellId,                 // 11
        // sekcja 5 dopisze: u => u.Uid, // 12
    };
    static readonly Action<SaveManager.SaveUnit, int>[] Set =
    {
        (u, v) => u.Cost = v,
        (u, v) => u.RealCost = v,
        (u, v) => u.Initiative = v,
        (u, v) => u.MaxHealth = v,
        (u, v) => { },                  // Health liczone po pętli
        (u, v) => u.Attack = v,
        (u, v) => u.Defense = v,
        (u, v) => u.Range = v,
        (u, v) => u.AP = v,
        (u, v) => u.MagicResist = v,
        (u, v) => u.UpgradeLevel = v,
        (u, v) => u.SpellId = v,
        // sekcja 5 dopisze: (u, v) => u.Uid = v,
    };

    // =====================================================================

    /// <summary>Dowolny tekst składu -> dane. Rozpoznaje stary JSON i nowy format.</summary>
    public static SaveManager.SaveData Parse(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return null;
        text = text.Trim();
        try
        {
            if (text.StartsWith(Prefix, StringComparison.Ordinal)) return Decode(text);
            if (text[0] == '{') return JsonUtility.FromJson<SaveManager.SaveData>(text);
        }
        catch (Exception e)
        {
            Debug.LogWarning("Nie udało się odczytać składu: " + e.Message);
        }
        return null;
    }

    public static string Encode(SaveManager.SaveData d)
    {
        using (var ms = new MemoryStream())
        using (var w = new BinaryWriter(ms))
        {
            w.Write(Version);
            WriteString(w, d.playerName ?? "");

            var header = new List<int> { d.number, d.bohater, d.levelUp };
            for (int i = 0; i < 3; i++)
                header.Add(d.lineLevels != null && i < d.lineLevels.Count ? d.lineLevels[i] : 0);
            header.Add(d.bohaterLevel);

            WriteVar(w, (uint)header.Count);
            foreach (int h in header) WriteZig(w, h);

            var units = d.units ?? new List<SaveManager.SaveUnit>();
            WriteVar(w, (uint)units.Count);
            foreach (var u in units)
            {
                WriteVar(w, (uint)((u.Pole << 3) | (u.Line + 1)));
                WriteVar(w, (uint)u.Id);

                uint mask = 0;
                for (int i = 0; i < Get.Length; i++)
                    if (Get[i](u) != 0) mask |= 1u << i;
                WriteVar(w, mask);

                for (int i = 0; i < Get.Length; i++)
                    if ((mask & (1u << i)) != 0) WriteZig(w, Get[i](u));
            }
            w.Flush();
            return Prefix + ToBase64Url(ms.ToArray());
        }
    }

    public static SaveManager.SaveData Decode(string text)
    {
        byte[] bytes = FromBase64Url(text.Substring(Prefix.Length));
        using (var r = new BinaryReader(new MemoryStream(bytes)))
        {
            byte version = r.ReadByte();
            if (version != Version) throw new FormatException("Nieznana wersja składu: " + version);

            var d = new SaveManager.SaveData { playerName = ReadString(r), lineLevels = new List<int>() };

            int n = (int)ReadVar(r);
            var header = new int[n];
            for (int i = 0; i < n; i++) header[i] = ReadZig(r);
            Func<int, int> H = idx => idx < n ? header[idx] : 0;

            d.number = H(HEADER_NUMBER);
            d.bohater = H(HEADER_BOHATER);
            d.levelUp = H(HEADER_LEVELUP);
            for (int i = 0; i < 3; i++) d.lineLevels.Add(H(HEADER_LINE0 + i));
            if (d.lineLevels.TrueForAll(l => l == 0)) d.lineLevels = null; // brak danych -> użyj levelUp
            d.bohaterLevel = H(HEADER_BOHATER_LEVEL);

            int count = (int)ReadVar(r);
            for (int k = 0; k < count; k++)
            {
                var u = new SaveManager.SaveUnit();
                uint pos = ReadVar(r);
                u.Line = (int)(pos & 7) - 1;
                u.Pole = (int)(pos >> 3);
                u.Id = (int)ReadVar(r);

                uint mask = ReadVar(r);
                int missingHp = 0;
                for (int i = 0; i < 32; i++)
                {
                    if ((mask & (1u << i)) == 0) continue;
                    int v = ReadZig(r);
                    if (i == F_HEALTH_MISSING) missingHp = v;
                    else if (i < Set.Length) Set[i](u, v);
                    // i >= Set.Length: pole z nowszej wersji gry – pomijamy
                }
                u.Health = u.MaxHealth - missingHp;
                d.units.Add(u);
            }
            return d;
        }
    }

    // ---- pomocnicze ----

    static void WriteVar(BinaryWriter w, uint v)
    {
        while (v >= 0x80) { w.Write((byte)(v | 0x80)); v >>= 7; }
        w.Write((byte)v);
    }

    static uint ReadVar(BinaryReader r)
    {
        uint v = 0; int shift = 0; byte b;
        do
        {
            b = r.ReadByte();
            v |= (uint)(b & 0x7F) << shift;
            shift += 7;
        } while ((b & 0x80) != 0 && shift < 35);
        return v;
    }

    static void WriteZig(BinaryWriter w, int v) => WriteVar(w, (uint)((v << 1) ^ (v >> 31)));
    static int ReadZig(BinaryReader r) { uint v = ReadVar(r); return (int)(v >> 1) ^ -(int)(v & 1); }

    static void WriteString(BinaryWriter w, string s)
    {
        byte[] b = Encoding.UTF8.GetBytes(s);
        WriteVar(w, (uint)b.Length);
        w.Write(b);
    }

    static string ReadString(BinaryReader r)
    {
        int len = (int)ReadVar(r);
        return Encoding.UTF8.GetString(r.ReadBytes(len));
    }

    static string ToBase64Url(byte[] b) => Convert.ToBase64String(b).TrimEnd('=').Replace('+', '-').Replace('/', '_');

    static byte[] FromBase64Url(string s)
    {
        s = s.Replace('-', '+').Replace('_', '/');
        switch (s.Length % 4) { case 2: s += "=="; break; case 3: s += "="; break; }
        return Convert.FromBase64String(s);
    }
}
