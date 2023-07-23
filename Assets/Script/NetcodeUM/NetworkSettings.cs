using UnityEngine;
using UnityMugen;

namespace UnityMugen.NetcodeUM
{

    public class NetworkSettings : MonoBehaviour
    {
        [Header("Network Options")]
        public int portServer = 6002;
        public int portHostClient = 6001;

        [Header("Advanced Options")]
        public int fps = 60;

        [Header("Debug Network")]
        public bool debugMode;
        public bool emulateNetwork;
        public bool trainingModeDebugger;
        public bool preloadedObjects;
        public bool networkToggle;
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
        public int maxBufferSize = 30;
        public NetworkRollbackBalancing rollbackBalancing = NetworkRollbackBalancing.Conservative;

        public bool disconnectOnDesynchronization;
        public bool desynchronizationRecovery;
        public int allowedDesynchronizations;
        public float floatDesynchronizationThreshold = 0.5f;


        [Header("Frame Delay Netcode")]
        public NetworkFrameDelay frameDelayType = NetworkFrameDelay.Fixed;
        public int defaultFrameDelay = 6;
        public int minFrameDelay = 4;
        public int maxFrameDelay = 30;
        public bool applyFrameDelayOffline;

    }
}