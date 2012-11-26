using System;

namespace EpochTime
{
    public class EpochTime
    {
        private static readonly DateTime Epoch = new DateTime( 1970, 1, 1, 0, 0, 0, DateTimeKind.Utc );

        public EpochTime( long epochTime, Unit unit = Unit.Seconds )
        {
            Time = unit == Unit.Seconds ? epochTime * 1000 : epochTime;
        }

        public EpochTime( DateTime dateTime )
        {
            Time = (long) TimeZoneInfo.ConvertTimeToUtc( dateTime )
                                    .Subtract( new DateTime( 1970, 1, 1, 0, 0, 0, DateTimeKind.Utc ) )
                                    .TotalMilliseconds;
        }

        public enum Unit
        {
            Seconds,
            Milliseconds
        }

        public long Time { get; private set; }

        public DateTime ToDateTime()
        {
            return Epoch.AddMilliseconds( Time );
        }

        public static EpochTime Parse( string s, Unit unit = Unit.Seconds )
        {
            long epochTime;
            if ( long.TryParse( s, out epochTime ) )
            {
                return new EpochTime( epochTime, unit );
            }
            return DateTime.Parse( s );
        }

        public bool Equals( EpochTime other )
        {
            if ( ReferenceEquals( null, other ) ) return false;
            if ( ReferenceEquals( this, other ) ) return true;
            return other.Time == Time;
        }

        public override bool Equals( object obj )
        {
            if ( ReferenceEquals( null, obj ) ) return false;
            if ( ReferenceEquals( this, obj ) ) return true;
            if ( obj.GetType() != typeof( EpochTime ) ) return false;
            return Equals( (EpochTime) obj );
        }

        public override int GetHashCode()
        {
            return Time.GetHashCode();
        }

        public static bool operator ==( EpochTime left, EpochTime right )
        {
            return Equals( left, right );
        }

        public static bool operator !=( EpochTime left, EpochTime right )
        {
            return !Equals( left, right );
        }

        public static implicit operator DateTime( EpochTime epochTime )
        {
            return epochTime.ToDateTime();
        }

        public static implicit operator EpochTime( DateTime dateTime )
        {
            return new EpochTime( dateTime );
        }

        public static EpochTime operator -( EpochTime left, EpochTime right )
        {
            return new EpochTime( left.Time - right.Time, Unit.Milliseconds );
        }

        public static EpochTime operator +( EpochTime left, EpochTime right )
        {
            return new EpochTime( left.Time + right.Time, Unit.Milliseconds );
        }
    }
}
