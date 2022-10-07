using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VertrouwenPuzzel
{
    public enum ActivePlayer
    {
        Player1,
        Player2
    }
    public enum SnowState
    {
        LowState,
        MidState,
        HeavyState
    }
    public class VertrouwenRoomManager : MonoBehaviour
    {
        [SerializeField]
        private FrostEffect _frost;
        [SerializeField]
        private SnowState _state;

        public static float _maxFrostAmount;
        public static float _frostAmount;

        public static float _lowAmount;
        public static float _midAmount;

        [SerializeField]
        private float _tempMaxFrost = 20;

        private void Start()
        {
            SetData(_tempMaxFrost);
            //Debug (gamemanager or player should handle this code)
            CheckPlayerCount.IncreasePlayerCount();
            //
        }
        private void Update()
        {
            /*if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();
            if (Input.GetKeyDown(KeyCode.R))
                ResetRoom();*/
            UpdateSnow();
        }
        private void UpdateSnow()
        {
            if (_frostAmount >= _maxFrostAmount)
            {
                _frostAmount = _maxFrostAmount;
                SwitchPlayer();
                return;
            }
            _frostAmount += Time.deltaTime;
            _frost.FrostAmount = _frostAmount / _maxFrostAmount;
        }
        public void SwitchPlayer()
        {
            ResetRoom();
        }
        public void SetData(float maxAmount)
        {
            _maxFrostAmount = maxAmount;
            _lowAmount = _maxFrostAmount * 0.33f;
            _midAmount = _maxFrostAmount * 0.66f;
        }
        private void ResetRoom()
        {
            _frostAmount = 0;
        }
    }

    
}
