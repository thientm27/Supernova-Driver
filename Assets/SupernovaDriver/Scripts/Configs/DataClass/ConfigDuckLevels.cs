using DuckSurvivor.Scripts.Configs;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;

public class ConfigDuckLevels : SgConfigDataTable<ConfigDuckLevelRecord>
{
    protected override void RebuildIndex()
    {
        RebuildIndexByField<int>("Id");
    }

    public ConfigDuckLevelRecord GetConfigDuckLevelById(int id)
    {
        ConfigDuckLevelRecord record = Records.FirstOrDefault(x => x.Id == id);
        return record;
    }

    public List<ConfigDuckLevelRecord> GetAllConfigDuckLevels()
    {
        return Records;
    }
}

public class ConfigDuckLevelRecord
{
    public int Id;
    public int NextLevelXP;
    public int TotalXPAtLevel;
}
