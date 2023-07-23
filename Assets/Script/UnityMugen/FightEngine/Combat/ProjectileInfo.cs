namespace UnityMugen.Combat
{
    public class ProjectileInfo
    {
        public ProjectileInfo()
        {
            m_projectileid = 0;
            m_datatype = ProjectileDataType.None;
            m_time = 0;
        }

        public ProjectileInfo(ProjectileInfo projectileInfo)
        {
            m_projectileid = projectileInfo.ProjectileId;
            m_datatype = projectileInfo.Type;
            m_time = projectileInfo.Time;
        }

        public void ResetFE()
        {
            m_projectileid = 0;
            m_datatype = ProjectileDataType.None;
            m_time = 0;
        }

        public void Set(long projId, ProjectileDataType datatype)
        {
            m_projectileid = projId;
            m_datatype = datatype;
            m_time = 0;
        }

        public void UpdateFE()
        {
            if (Type != ProjectileDataType.None) ++m_time;
        }

        public long ProjectileId => m_projectileid;
        public ProjectileDataType Type => m_datatype;
        public int Time => m_time;

        private long m_projectileid;
        private ProjectileDataType m_datatype;
        private int m_time;

    }
}