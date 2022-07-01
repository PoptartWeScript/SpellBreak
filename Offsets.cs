using System;
namespace SpellBreak
{

public class Offsets

{
   public static Int64 UWorld = 0x739F198;
   public static Int64 GNames = 0x74179D0;

public class UE
{

   public class UWorld
{
       public static Int64 ULevel = 0x38;
       public static Int64 OwningGameInstance = 0x170;
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
}

   public class APlayerController
{
       public static Int64 AcknowledgedPawn = 0x3D0;
       public static Int64 PlayerCameraManager = 0x3E8;
}

   public class APawn
{
       public static Int64 PlayerState = 0x368;
}
   public class AActor
{
       public static Int64 USceneComponent = 0x168;
       public static Int64 tempVec= 0x17C;
}

   public class APlayerCameraManager
{
       public static Int64 CameraCachePrivate = 0x1A90;
}

}
}
}
