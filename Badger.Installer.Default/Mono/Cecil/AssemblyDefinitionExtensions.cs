using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mono.Cecil {
    public static class AssemblyDefinitionExtensions {
        public static bool AddFromObject<T>(this ICollection<Resource> This, string ResourceName, T Content) {
            var ret = false;

            try {
                var ResourceContent = Badger.Installer.ResourceSerializer.ToByte(Content);
                ret = This.AddFromBytes(ResourceName, ResourceContent);
            } catch (Exception ex) { 
            
            }

            return ret;
        }
        
        public static bool AddFromFile(this ICollection<Resource> This, string ResourceName,  string FileName) {
            var ret = false;

            try {

                if (System.IO.File.Exists(FileName)) {
                    var ResourceContent = System.IO.File.ReadAllBytes(FileName);
                    ret = This.AddFromBytes(ResourceName, ResourceContent);
                }
            } catch (Exception ex){
                
            }

            return ret;
        }

        public static bool AddFromBytes(this ICollection<Resource> This, string ResourceName, byte[] ResourceContent) {
            var ret = false;

            try {
                This.Add(new Mono.Cecil.EmbeddedResource(ResourceName, Mono.Cecil.ManifestResourceAttributes.Public, ResourceContent));
                ret = true;
            } catch (Exception ex) {

            }

            return ret;
        }


    }
}
