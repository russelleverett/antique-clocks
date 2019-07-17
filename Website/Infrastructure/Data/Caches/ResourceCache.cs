using System;
using System.Collections.Generic;
using Website.Infrastructure.Data.Entities;
using Website.Infrastructure.Services;

namespace Website.Infrastructure.Data.Caches {
    public interface IResourceCache : ICache<Resource>, IDatabaseCache<Resource> {
        IEnumerable<Resource> GetAll();
        Resource GetDefaultClockImage(int clockId, int parentTypeId = 0);
        IEnumerable<Resource> GetClockImages(int clockId);
        Resource GetClockAudio(int clockId);
    }

    public class ResourceCache : DatabaseCache<Resource>, IResourceCache {
        public ResourceCache(IDomainContext context) : base(context) {
        }

        public IEnumerable<Resource> GetAll() {
            return new List<Resource>(_cache.Values);
        }

        public Resource GetDefaultClockImage(int clockId, int parentTypeId = 0) {
            foreach (var resource in _cache.Values) {
                if (resource.ClockId == clockId && resource.Default && resource.ParentTypeId == parentTypeId) {
                    return resource;
                }
            }
            return null;
        }

        public IEnumerable<Resource> GetClockImages(int clockId) {
            List<Resource> resources = new List<Resource>();
            foreach (var resource in _cache.Values) {
                if (resource.ClockId == clockId && resource.FileType == FileType.Image && resource.ParentTypeId == 0) {
                    resources.Add(resource);
                }
            }
            return resources;
        }

        public Resource GetClockAudio(int clockId) {
            foreach (var resource in _cache.Values) {
                if (resource.ClockId == clockId && resource.FileType == FileType.Audio) {
                    return resource;
                }
            }
            return null;
        }
    }
}
