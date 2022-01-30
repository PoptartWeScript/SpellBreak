using System;
namespace SpellBreak
{
    public class Offsets

    {

        public static Int64 GObjects = 0x7A7DAE0;
        public static Int64 GNames = 0x7A31CC0;
        public static Int64 UWorld = 0x8027F90;

        public class UE
        {
            public class UWorld
            {
                public static Int64 PersistentLevel = 0x38; // public class ULevel*
                public static Int64 NetworkManager = 0x60; // public class AGameNetworkManager*
                public static Int64 OwningGameInstance = 0x170;// public class UGameInstance*
                public static Int64 gameState = 0x0140;
            }

            public class ULevel
            {
                public static Int64 AActors = 0xA0;
                public static Int64 AActorsCount = 0xA8;
            }

            public class UGameInstance
            {
                public static Int64 LocalPlayers = 0x40;
            }

            public class UPlayer
            {
                public static Int64 PlayerController = 0x38;
                public static Int64 CurrentNetSpeed = 0x40;
            }

            public class APlayerController
            {
                public static Int64 AcknowledgedPawn = 0x3D0;
                public static Int64 PlayerCameraManager = 0x3E8;
            }

            public class AController
            {
                public static Int64 PlayerState = 0x348;
                public static Int64 Pawn = 0x378;
                public static Int64 Character = 0x388;
                public static Int64 TransformComponent = 0x390;
                public static Int64 ControlRotation = 0x3B0;
                public static Int32 stateName = 0x0258;

            }

            public class APawn
            {
                public static Int64 PlayerState = 0x368;
                public static Int64 Controller = 0x378;
                public static Int64 Health = 0x830;
                public static Int64 MaxHealth = 0x850;
            }

            public class APlayerState
            {
                public static Int64 PlayerId = 0x368;
                public static Int64 Ping = 0x36C;
                public static Int64 UniqueId = 0x398;
                public static Int64 PlayerNamePrivate = 0x450;
                public static Int64 Team = 0x4C9;
            }

            public class AKSTeamState
            {
                public static Int64 r_TeamNum = 0x220;
            }

            public class AActor
            {
                public static Int64 Instigator = 0x0150;
                public static Int64 RootComponent = 0x168;

            }

            public class ACharacter
            {
                public static Int64 Mesh = 0x3A0;
                public static Int64 CharacterMovement = 0x3A8;
            }

            // from ACharacter to here
            public class UCharacterMovementComponent
            {
                public static Int64 GravityScale = 0x150;
                public static Int64 JumpZVelocity = 0x158;
                public static Int64 MaxWalkSpeed = 0x18C;
                public static Int64 MaxWalkSpeedCrouched = 0x190;
                public static Int64 MaxSwimSpeed = 0x194;
                public static Int64 MaxFlySpeed = 0x198;
                public static Int64 MaxAcceleration = 0x1A0;
            }

            public class USceneComponent
            {
                public static Int64 BoundsOrigin = 0x100; // vec3
                public static Int64 BoundsExtended = 0x10C; // vec3
                public static Int64 BoundsRadius = 0x118; // float
                public static Int64 RelativeLocation = 0x17C; // struct FVector
                public static Int64 RelativeRotation = 0x188; // struct FRotator
                public static Int64 ComponentVelocity = 0x1D0; // struct FVector
            }

            public class UStaticMeshComponent
            {
                public static Int64 ComponentToWorld = 0x1E0; //Bone Array
                public static Int64 StaticMesh = 0x490; // public class UStaticMesh*
            }

            public class USkinnedMeshComponent
            {
                public static Int64 SkeletalMesh = 0x5B0; // public class USkeletalMesh*
                public static Int64 bDisplayBones = 0x6C6; // Bool
                public static Int64 bRecentlyRendered = 0x6C7; // Bool
                public static Int64 CachedWorldSpaceBounds = 0x4A0; // FTransform 450 / 470 (0x00F8) MISSED OFFSET
            }

            public class USkeletalMesh
            {
                public static Int64 Skeleton = 0x50; // public class USkeleton*
            }

            public class USkeleton
            {
                public static Int64 BoneTree = 0x40; // TArray<struct FBoneNode>
                public static Int64 VirtualBoneGuid = 0x178; // struct FGuid
                public static Int64 VirtualBones = 0x188; //TArray<struct FVirtualBone>
            }

            public class APlayerCameraManager
            {
                public static Int64 TransformComponent = 0x348; // public class USceneComponent*
                public static Int64 CameraCachePrivate = 0x1A70; // struct FCameraCacheEntry
                public static Int64 CurrentFov = 0x1A98; // float
                public static Int64 AnimCameraActor = 0x2698; // public class ACameraActor*
            }

            // from APlayerCameraManager to here
            public class ACameraActor
            {
                public static Int64 CameraComponent = 0x348; // public class UCameraComponent*
            }

            // from ACameraActor to here
            public class UCameraComponent
            {
                public static Int64 FieldOfView = 0x1F0;
            }
        }
    }
}
