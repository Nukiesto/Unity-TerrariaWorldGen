using MessagePack;
using MessagePack.Resolvers;
using UnityEngine;

namespace Game.Serialization.MessagePackGenerated.c
{
    public class Startup
    {
        private static bool _serializerRegistered = false;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            if (!_serializerRegistered)
            {
                StaticCompositeResolver.Instance.Register(
                    MessagePack.Resolvers.GeneratedResolver.Instance,
                    MessagePack.Resolvers.StandardResolver.Instance
                );

                MessagePackSerializerOptions option = MessagePackSerializerOptions.Standard.WithResolver(StaticCompositeResolver.Instance);

                MessagePackSerializer.DefaultOptions = option;
                _serializerRegistered = true;
            }
        }

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
        private static void EditorInitialize()
        {
            Initialize();
        }
#endif
    }
}