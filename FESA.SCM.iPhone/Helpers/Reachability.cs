using System;
using System.Net;
using SystemConfiguration;
using CoreFoundation;

namespace FESA.SCM.iPhone.Helpers
{
    public enum NetworkStatus
    {
        NotReachable,
        ReachableViaCarrierDataNetwork,
        ReachableViaWiFiNetwork
    }

    public static class Reachability
    {
        public static string HostName = "www.google.com";

        public static bool IsReachableWithoutRequiringConnection(NetworkReachabilityFlags flags)
        {
            // Is it reachable with the current network configuration?
            var isReachable = (flags & NetworkReachabilityFlags.Reachable) != 0;

            // Do we need a connection to reach it?
            var noConnectionRequired = (flags & NetworkReachabilityFlags.ConnectionRequired) == 0
                || (flags & NetworkReachabilityFlags.IsWWAN) != 0;

            return isReachable && noConnectionRequired;
        }

        // Is the host reachable with the current network configuration
        public static bool IsHostReachable(string host)
        {
            if (string.IsNullOrEmpty(host))
                return false;

            using (var r = new NetworkReachability(host))
            {
                NetworkReachabilityFlags flags;

                if (r.TryGetFlags(out flags))
                    return IsReachableWithoutRequiringConnection(flags);
            }
            return false;
        }

        //
        // Raised every time there is an interesting reachable event,
        // we do not even pass the info as to what changed, and
        // we lump all three status we probe into one
        //
        public static event EventHandler ReachabilityChanged;

        static void OnChange(NetworkReachabilityFlags flags)
        {
            var h = ReachabilityChanged;
            h?.Invoke(null, EventArgs.Empty);
        }

        //
        // Returns true if it is possible to reach the AdHoc WiFi network
        // and optionally provides extra network reachability flags as the
        // out parameter
        //
        private static NetworkReachability _adHocWiFiNetworkReachability;

        public static bool IsAdHocWiFiNetworkAvailable(out NetworkReachabilityFlags flags)
        {
            if (_adHocWiFiNetworkReachability != null)
                return _adHocWiFiNetworkReachability.TryGetFlags(out flags) &&
                       IsReachableWithoutRequiringConnection(flags);
            _adHocWiFiNetworkReachability = new NetworkReachability(new IPAddress(new byte[] { 169, 254, 0, 0 }));
            _adHocWiFiNetworkReachability.SetNotification(OnChange);
            _adHocWiFiNetworkReachability.Schedule(CFRunLoop.Current, CFRunLoop.ModeDefault);

            return _adHocWiFiNetworkReachability.TryGetFlags(out flags) && IsReachableWithoutRequiringConnection(flags);
        }

        private static NetworkReachability _defaultRouteReachability;

        private static bool IsNetworkAvailable(out NetworkReachabilityFlags flags)
        {
            if (_defaultRouteReachability != null)
                return _defaultRouteReachability.TryGetFlags(out flags) && IsReachableWithoutRequiringConnection(flags);
            _defaultRouteReachability = new NetworkReachability(new IPAddress(0));
            _defaultRouteReachability.SetNotification(OnChange);
            _defaultRouteReachability.Schedule(CFRunLoop.Current, CFRunLoop.ModeDefault);
            return _defaultRouteReachability.TryGetFlags(out flags) && IsReachableWithoutRequiringConnection(flags);
        }

        private static NetworkReachability _remoteHostReachability;

        public static NetworkStatus RemoteHostStatus()
        {
            NetworkReachabilityFlags flags;
            bool reachable;

            if (_remoteHostReachability == null)
            {
                _remoteHostReachability = new NetworkReachability(HostName);

                // Need to probe before we queue, or we wont get any meaningful values
                // this only happens when you create NetworkReachability from a hostname
                reachable = _remoteHostReachability.TryGetFlags(out flags);

                _remoteHostReachability.SetNotification(OnChange);
                _remoteHostReachability.Schedule(CFRunLoop.Current, CFRunLoop.ModeDefault);
            }
            else
            {
                reachable = _remoteHostReachability.TryGetFlags(out flags);
            }

            if (!reachable)
                return NetworkStatus.NotReachable;

            if (!IsReachableWithoutRequiringConnection(flags))
                return NetworkStatus.NotReachable;

            return (flags & NetworkReachabilityFlags.IsWWAN) != 0 ?
                NetworkStatus.ReachableViaCarrierDataNetwork : NetworkStatus.ReachableViaWiFiNetwork;
        }

        public static NetworkStatus InternetConnectionStatus()
        {
            NetworkReachabilityFlags flags;
            var defaultNetworkAvailable = IsNetworkAvailable(out flags);
            if (defaultNetworkAvailable && ((flags & NetworkReachabilityFlags.IsDirect) != 0))
                return NetworkStatus.NotReachable;
            if ((flags & NetworkReachabilityFlags.IsWWAN) != 0)
                return NetworkStatus.ReachableViaCarrierDataNetwork;
            return flags == 0 ? NetworkStatus.NotReachable : NetworkStatus.ReachableViaWiFiNetwork;
        }

        public static NetworkStatus LocalWifiConnectionStatus()
        {
            NetworkReachabilityFlags flags;
            if (!IsAdHocWiFiNetworkAvailable(out flags)) return NetworkStatus.NotReachable;
            return (flags & NetworkReachabilityFlags.IsDirect) != 0 ? NetworkStatus.ReachableViaWiFiNetwork : NetworkStatus.NotReachable;
        }
    }
}