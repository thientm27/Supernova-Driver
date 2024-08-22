using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DuckSurvivor.Scripts.Configs
{
    public class ConfigWaveDetail : SgConfigDataTable<ConfigWaveDetailRecord>
    {
        protected override void RebuildIndex()
        {
            RebuildIndexByField<int>("WaveID");
        }

        public List<ConfigWaveDetailRecord> GetAllConfigByWaveId(int id)
        {
            List<ConfigWaveDetailRecord> records = Records.FindAll(x => x.WaveID == id);
            return records;
        }

        public List<ConfigWaveDetailRecord> GetAllConfigWaveDetail()
        {
            return Records;
        }

        public Dictionary<int, int> GetDicMonsterSpawnRatio(int waveID)
        {
            Dictionary<int, int> newDic = new Dictionary<int, int>();
            //Find all wave ID
            List<ConfigWaveDetailRecord> lstWaveID = GetAllConfigByWaveId(waveID);
            foreach(ConfigWaveDetailRecord record in lstWaveID)
            {
                newDic.Add(record.MonsterID, record.Weight);
            }    
            return newDic;
        }
    }

    public class ConfigWaveDetailRecord
    {
        public int WaveID;
        public int MonsterID;
        public int Weight;
    }
}