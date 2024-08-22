using DuckSurvivor.Scripts.Configs;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;

public class ConfigDucks : SgConfigDataTable<ConfigDucksRecord>
{
    protected override void RebuildIndex()
    {
        RebuildIndexByField<int>("hb");
    }

    public ConfigDucksRecord GetConfigSkillById(int id)
    {
        ConfigDucksRecord record = Records.FirstOrDefault(x => x.hb == id);
        return record;
    }

    public List<ConfigDucksRecord> GetAllConfigDucks()
    {
        return Records;
    }
}

public class ConfigDucksRecord
{
    public int hb;
    public int Type;
    public int Level;
    public string Model;
    public int ATK;
    public int HP;
    public int Speed;
    public int Range;
    public float CritChance;
    public float CritDamage;
    public int BasePrice;
    public string PriceIncrement;
}
