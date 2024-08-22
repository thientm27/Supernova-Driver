
using DuckSurvivor.Scripts.Configs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace DuckSurvivor.Scripts.Configs.DataClass
{
    public class LevelConfig : SgConfigDataTable<ConfigLevelRecord>
    {
        protected override void RebuildIndex()
        {
            RebuildIndexByField<int>("Id");
        }

        public ConfigLevelRecord GetConfigLevelById(int id)
        {
            ConfigLevelRecord record = Records.FirstOrDefault(x => x.Id == id);
            return record;
        }

        public List<ConfigLevelRecord> GetAllConfigLevel()
        {
            return Records;
        }

        public int GetMaxLevel()
        {
            return Records[^1].Id;
        }
    }

    public class ConfigLevelRecord
    {
        public int Id;
        public string Waves;
        public int Boss;
        public int EnemyLimit;
    }
}

