using Semver;
using System;

namespace MOP.Core.Infra
{
    public sealed class MopVersion : IComparable<MopVersion>, IComparable
    {
        private SemVersion _version = new SemVersion(1, 0, 0);

        public int Major { 
            get => _version.Major;
            set => Update(value, Minor, Patch);
        }

        public int Minor
        {
            get => _version.Minor;
            set => Update(Major, value, Patch);
        }

        public int Patch
        {
            get => _version.Patch;
            set => Update(Major, Minor, value);
        }

        public MopVersion() { }

        public MopVersion(int major, int minor, int patch)
            => Update(major, minor, patch);

        public MopVersion(string value)
            => _version = SemVersion.Parse(value);

        public void Update(int major, int minor, int patch)
            => _version = new SemVersion(major, minor, patch);

        public int CompareTo(object obj)
        {
            if (obj is MopVersion v)
                return CompareTo(v);
            throw new ArgumentException("obj is not an MopVersion");
        }

        public int CompareTo(MopVersion other)
            => _version.CompareTo(other._version);
    }
}
