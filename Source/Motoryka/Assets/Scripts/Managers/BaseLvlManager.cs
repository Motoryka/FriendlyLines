using UnityEngine;

public enum LevelPhase
{
    Paused,
    Prestarted,
    Started,
    Running,
    Prefinished,
    Finished
}

public abstract class BaseLvlManager<T> : Singleton<T> where T : MonoBehaviour
{
    LevelPhase _phase = LevelPhase.Prestarted;

    public LevelPhase CurrentPhase {
        get
        {
            return _phase;
        }
        set
        {
            if (_phase == LevelPhase.Running && value == LevelPhase.Paused)
            {
                _phase = value;
            }
            else if (_phase == LevelPhase.Paused && value == LevelPhase.Running)
            {
                _phase = value;
            }
            else if (_phase == LevelPhase.Prestarted && value ==LevelPhase.Started)
            {
                PreStart();
                _phase = value;
            }
            else if (_phase == LevelPhase.Prestarted && value == LevelPhase.Running)
            {
                PreStart();
                PostStart();
                _phase = value;
            }
            else if (_phase == LevelPhase.Started && value == LevelPhase.Running)
            {
                PostStart();
                _phase = value;
            }
            else if (_phase == LevelPhase.Running && value == LevelPhase.Prefinished)
            {
                PreFinish();
                _phase = value;
            }
            else if (_phase == LevelPhase.Running && value == LevelPhase.Finished)
            {
                PreFinish();
                PostFinish();
                _phase = value;
            }
            else if (_phase == LevelPhase.Prefinished && value == LevelPhase.Finished)
            {
                PostFinish();
                _phase = value;
            }
        }
    }

    protected virtual void PreStart()
    {

    }

    protected virtual void PostStart()
    {

    }

    protected virtual void PreFinish()
    {

    }

    protected virtual void PostFinish() 
    {

    }

}

