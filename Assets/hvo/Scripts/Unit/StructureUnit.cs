
using JetBrains.Annotations;

public class StructureUnit : Unit//

    //建筑单位类，继承自Unit类
    //该类用于处理建筑单位的相关逻辑和状态


    //建筑进程对象，用于管理建筑的建造过程
    //private BuildingProcess m_BuildingProcess;
{
    private BuildingProcess m_BuildingProcess;


    public bool IsUnderConstruction => m_BuildingProcess != null; //是否正在建造中


    void Update()
    {
        if(IsUnderConstruction)
        {
            m_BuildingProcess.Update(); //更新建造进程
        }
    }

    public void RegisterProcess(BuildingProcess process)
    {
        m_BuildingProcess = process; //注册建筑进程
    }
}