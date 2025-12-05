using System;

// Token: 0x02000006 RID: 6
internal class SimpleInputs
{
    // Token: 0x17000002 RID: 2
    // (get) Token: 0x06000011 RID: 17 RVA: 0x00002BF4 File Offset: 0x00000DF4
    public static bool RightTrigger
    {
        get
        {
            return ControllerInputPoller.instance.rightControllerIndexFloat > 0.5f;
        }
    }

    // Token: 0x17000003 RID: 3
    // (get) Token: 0x06000012 RID: 18 RVA: 0x00002C09 File Offset: 0x00000E09
    public static bool RightGrab
    {
        get
        {
            return ControllerInputPoller.instance.rightGrab;
        }
    }

    // Token: 0x17000004 RID: 4
    // (get) Token: 0x06000013 RID: 19 RVA: 0x00002C17 File Offset: 0x00000E17
    public static bool RightA
    {
        get
        {
            return ControllerInputPoller.instance.rightControllerSecondaryButton;
        }
    }

    // Token: 0x17000005 RID: 5
    // (get) Token: 0x06000014 RID: 20 RVA: 0x00002C25 File Offset: 0x00000E25
    public static bool RightB
    {
        get
        {
            return ControllerInputPoller.instance.rightControllerSecondaryButton;
        }
    }

    // Token: 0x17000006 RID: 6
    // (get) Token: 0x06000015 RID: 21 RVA: 0x00002C33 File Offset: 0x00000E33
    public static bool LeftTrigger
    {
        get
        {
            return ControllerInputPoller.instance.leftControllerIndexFloat > 0.5f;
        }
    }

    // Token: 0x17000007 RID: 7
    // (get) Token: 0x06000016 RID: 22 RVA: 0x00002C48 File Offset: 0x00000E48
    public static bool LeftGrab
    {
        get
        {
            return ControllerInputPoller.instance.leftGrab;
        }
    }

    // Token: 0x17000008 RID: 8
    // (get) Token: 0x06000017 RID: 23 RVA: 0x00002C56 File Offset: 0x00000E56
    public static bool LeftX
    {
        get
        {
            return ControllerInputPoller.instance.leftControllerPrimaryButton;
        }
    }

    // Token: 0x17000009 RID: 9
    // (get) Token: 0x06000018 RID: 24 RVA: 0x00002C64 File Offset: 0x00000E64
    public static bool LeftY
    {
        get
        {
            return ControllerInputPoller.instance.leftControllerSecondaryButton;
        }
    }
}
