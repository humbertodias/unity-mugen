using UnityEngine;
using UnityMugen;

namespace UnityMugen.NetcodeUM
{
    [CreateAssetMenu(fileName = "NetworkSettings", menuName = "Unity Mugen/Settings/Network Settings")]
    public class NetworkSettings : ScriptableObject
    {
        [Header("Network Options")]
        public int portServer = 6002;
        public int portHostClient = 6001;

        [Header("Advanced Options")]
        public int fps = 60;

        [Header("Debug Network")]
        public bool debugMode = true;
        public bool emulateNetwork = true;
        public bool trainingModeDebugger = true;
        public bool preloadedObjects = true;
        public bool networkToggle = true;
        public bool connectionLog = true;

        [Header("Debug - Network Info")]
        public bool ping = true;
        public bool frameDelay = true;
        public bool currentLocalFrame = true;
        public bool currentNetworkFrame = true;
        public bool desyncErrorLog = false;
        public bool stateTrackerTest = false;

        [Header("Packege Options")]
        public NetworkMessageSize networkMessageSize = NetworkMessageSize.Size32Bits;
        public NetworkInputMessageFrequency inputMessageFrequency = NetworkInputMessageFrequency.EveryFrame;
        public bool onlySendInputChanges = true;
        public NetworkSynchronizationMessageFrequency synchronizationMessageFrequency = NetworkSynchronizationMessageFrequency.EverySecond;

        [Header("Rollback Netcode")]
        public bool allowRollBacks = true;
        public bool ufeTrackers = false;
        public int maxFastForwards = 10;
        public int maxBufferSize = 10;
        public NetworkRollbackBalancing rollbackBalancing = NetworkRollbackBalancing.Aggressive;

        public bool disconnectOnDesynchronization = true;
        public bool desynchronizationRecovery = true;
        public int allowedDesynchronizations = 0;
        public float floatDesynchronizationThreshold = 0.5f;


        [Header("Frame Delay Netcode")]
        public NetworkFrameDelay frameDelayType = NetworkFrameDelay.Fixed;
        public int defaultFrameDelay = 0;
        public int minFrameDelay = 1;
        public int maxFrameDelay = 7;
        public bool applyFrameDelayOffline = false;

    }
}