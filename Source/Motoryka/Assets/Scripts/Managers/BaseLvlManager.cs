/**********************************************************************
Copyright (C) 2015  Wojciech Nadurski

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/

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

public abstract class BaseLvlManager<T> : Singleton<T>, IInitable where T : MonoBehaviour, IInitable
{
    LevelPhase _phase = LevelPhase.Prestarted;
    bool initialized = false;
    bool _restarting = false;
    bool _finishing = false;
    
    protected ShapeElement shape;

    public virtual void Init()
    {
    }

    public void Initialize()
    {
        if (!initialized)
        {
            Init();
        }

        initialized = true;
    }

    void Start()
    {
        Initialize();
    }

    void Update()
    {
        if (CurrentPhase == LevelPhase.Prestarted)
        {
            CurrentPhase = LevelPhase.Running;
        }
    }
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
                _PreStart();
                _phase = value;
            }
            else if (_phase == LevelPhase.Prestarted && value == LevelPhase.Running)
            {
                _PreStart();
                _PostStart();
                _phase = value;
            }
            else if (_phase == LevelPhase.Started && value == LevelPhase.Running)
            {
                _PostStart();
                _phase = value;
            }
            else if (_phase == LevelPhase.Running && value == LevelPhase.Prefinished)
            {
                _PreFinish();
                _phase = value;
            }
            else if (_phase == LevelPhase.Running && value == LevelPhase.Finished)
            {
                _PreFinish();
                _PostFinish();
                _phase = value;
            }
            else if (_phase == LevelPhase.Prefinished && value == LevelPhase.Finished)
            {
                _PostFinish();
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

    private void _PreStart()
    {
        PreStart();
    }

    private void _PostStart()
    {
        PostStart();
        GameManager.Instance.SendMessage("StartedLevel"); //.StartedLevel();
    }

    private void _PreFinish()
    {
        PreFinish();
    }

    private void _PostFinish()
    {
        if (!_finishing && !_restarting)
        {
            _finishing = true;
            PostFinish();
            shape.DontPreserve();
            GameManager.Instance.SendMessage("FinishedLevel");
        }
    }

    public void RestartLevel()
    {
        if (!_finishing && !_restarting)
        {
            _restarting = true;
            shape.Preserve();
            GameManager.Instance.SendMessage("RestartLevel", shape);
        }
    }
}
