using UnityEngine;

namespace Imba.Audio
{
    [SerializeField]
    public enum AudioName
    {
        // Main Music
        Track_1 = -2,

        NoSound = -1,

        BGM_Menu     = 0,
        BGM_GAMEPLAY = 1,
        AMB_GAMEPLAY = 2,

        #region UI

        #endregion

        #region GAME PLAY

        Claim     = 30,
        Claim2X   = 32,
        CountDown = 33,
        StartGame = 34,

        Explotion      = 35,
        Explotion2     = 36,
        Explotion3     = 37,
        SoundCarStart1 = 38,
        SoundCarStart2 = 39,

        #endregion
    }

    public enum AudioType
    {
        SFX = 0,
        BGM = 1,
        AMB = 2,
    }
}