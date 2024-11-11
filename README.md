# Coroutine Tutorial

This tutorial demonstrates using a coroutine to create a clock

## Prerequisites

Before approaching this tutorial, you will need a current version of Unity and a code editor (such as Microsoft Visual Studio Community) installed and ready to use.

This tutorial was created with Unity 2022.3 LTS and Microsoft Visual Studio Community 2022 versions. It should work with earlier or later versions. But you should check the release notes for other versions as the Editor controls or Scripting API functions may have changed.

If you need help installing Unity you can find many online tutorials such as:
https://learn.unity.com/tutorial/install-the-unity-hub-and-editor

You will also need to know how to create an empty project, add and edit primitive objects to your scene, create materials, create blank scripts, and run projects from within the editor. If you need help with this, there is a short video demonstrating how to do most of these things here: 

https://www.youtube.com/watch?v=eQpWPfP1T6g

## Objectives

The objective of this tutorial is to show how to start and run a coroutine that updates the hands of a clock one a second.

https://github.com/user-attachments/assets/c280e06b-3ff0-4a04-a921-d0c41dc6eee8

## Create a scene

Create a new 3D Core Unity project.

To create our sample scene for this demonstration we'll need to create a hierarchy of objects that are not necessarily in their default orientation or offset from their default pivot point. We'll start by creating an empty object to represent the clock and call it "Clock". Point this object towards the camera so that later rotations will be clockwise rotations.

As a child of this object, add three empty children: `SecondHandPivot`, `MinuteHandPivot`, and `HourHandPivot`. Now we can add a cylinder as a fourth child, `FaceModel`, scaled to be a thin disc and rotated to face the camera. And as a child of each pivot add a cube to act as the respective hand model. Each should be long, thin, and offset relative to the pivot.

If you want to copy my setup, here are the transforms of the various objects:

| GameObject | position | rotation | scale |
| :--- | ---: | ---: | ---: |
| Clock | 0, 0, 0 | 0, 180, 0 | 1, 1, 1 |
| → FaceModel | 0, 0, 0 | 90, 0, 0 | 0.3, 0.001, 0.3 |
| → SecondHandPivot | 0, 0, 0 | 0, 0, 0 | 1, 1, 1 |
| →→ SecondHandModel | 0, 0.05, 0.03 | 0, 0, 0 | 0.002, 0.15, 0.001 |
| → MinuteHandPivot | 0, 0, 0 | 0, 0, 0 | 1, 1, 1 |
| →→ MinuteHandModel | 0, 0.05, 0.02 | 0, 0, 0 | 0.01, 0.13, 0.001 |
| → HourHandPivot | 0, 0, 0 | 0, 0, 0 | 1, 1, 1 |
| →→ HourHandModel | 0, 0.03, 0.01 | 0, 0, 0 | 0.02, 0.08, 0.001 |

It should look something like this:

![clock face made of primitive objects](https://github.com/user-attachments/assets/d0298d74-cece-47de-9fcb-58af74815583)

If you have set these models up correctly then rotating the `Z` value of the pivot objects should cause the respective hands to rotate around the clock face. Positive changes to `Z` should make the hand move clockwise.

## Create a clock script

Create a new script called `Clock` and add it to the Clock object. To have this script control the clock hands we need to connect it to each of the hand pivot transforms. To do this add a public transform variable at the top of the class definition, one for each hand:

```cs
public class Clock : MonoBehaviour
{
    public Transform secondHand;
    public Transform minuteHand;
    public Transform hourHand;
}
```

Now switch back to Unity and drag the respective pivot into each of the variable slots in the clock script in the inspector window.

![the clock script in the inspector window with each of the hand pivot transforms attached](https://github.com/user-attachments/assets/bc0a7951-7795-4c1a-917f-a37d6552e0ff)

## What is a coroutine?

We could update our clock every frame, but we only need to change the positions of the hands every second. Coroutines are functions that can execute a bit at a time. Each time we want the function to finish a step and wait, we yield a return value back to Unity. The function will pause execution until its next update.

To create a coroutine you need to create a function that returns an `IEnumerator` value.

```cs
    IEnumerator Tick()
    {
        yield return null;
    }
```

[`IEnumerator`](https://learn.microsoft.com/en-us/dotnet/api/system.collections.ienumerator?view=netstandard-2.0) is a special type of interface defined in the dot net library. When you define a function that returns this type, you must include at least one [`yield`](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/yield) statement within your function. This instruction is used to provide the next value or signal the end of an iterator function.

To call a coroutine function, we need to use the [`StartCoroutine`](https://docs.unity3d.com/2022.3/Documentation/ScriptReference/MonoBehaviour.StartCoroutine.html) function of `MonoBehaviour`.

Once started, the coroutine will continue to run so long as it is yielding new values. So we only need to start the coroutine once.

> [!WARNING]
> If you start a coroutine more than once, a new instance of the coroutine will start and will run in parallel with other instances of the same coroutine.

We can start our coroutine from the `Start` function as this will only happen once when we start our game.

```cs
    void Start()
    {
        StartCoroutine(Tick());
    }
```

The `StartCoroutine` function takes an `IEnumerator` value as its argument, so we use the `Tick` function to return its enumerator and that starts the coroutine. At this point, our coroutine doesn't do anything, so to check that it works, we can add a `Debug.Log` line to output to the console.

```cs
    IEnumerator Tick()
    {
        Debug.Log("Tick");
        yield return null;
    }
```

Switch back to Unity and run. You should see a single message `Tick` appear in the console.

## Creating an infinite loop

With normal functions, creating an infinite loop is bad. If you try this in a Unity script, Unity will hang and become unresponsive. You will need to kill the process and reopen Unity to get out of it.

![Unity.exe in the task manager once it has become unresponsive and needs to be terminated](https://github.com/user-attachments/assets/9fb77453-6263-47f7-8ecf-3192b73aa1b7)

However, as long as we yield a value inside an infinite loop in our coroutine, Unity will retain control and won't lock up.

```cs
    IEnumerator Tick()
    {
        while (true)
        {
            Debug.Log("Tick");
            yield return null;
        }
    }
```

Here, the instruction `while` will cause the program to loop, as long as the condition in the brackets is true. Because we are providing a constant `true` value, this loop will run forever.

Switch back to Unity and run. You should see a continuous stream of `Tick` messages appear in the console.

## Wait a second...

Although we now have a loop processing ticks, it is happening every frame rather than every second. Depending on the frame rate, this could be hundreds or thousands of times a second. To reduce the amount of processing we need to do, we can yield a new [`YieldInstruction`](https://docs.unity3d.com/2022.3/Documentation/ScriptReference/YieldInstruction.html) to wait for one second before continuing.

The [`WaitForSeconds`](https://docs.unity3d.com/2022.3/Documentation/ScriptReference/WaitForSeconds.html) class is a `YieldInstruction` that waits for the given number of seconds until continuing. We can do this by changing our `yield` instruction:

```cs
        while (true)
        {
            Debug.Log("Tick");
            yield return new WaitForSeconds(1);
        }
```

Here, the `new` statement says that we are creating a new object of the `WaitForSeconds` class. The initializer argument `(1)` is the time to wait in seconds.

> [!CAUTION]
> The time in seconds is Unity scaled time. If you change the time scale in the Unity settings, you can get the time to run faster or slower.

Switch back to Unity and run. You should now see one `Tick` message appearing in the console window each second.

## Moving the second hand

To rotate the hands we can use the [`Transform.Rotate`](https://docs.unity3d.com/2022.3/Documentation/ScriptReference/Transform.Rotate.html) function. We are going to use the version of the function that takes an axis and an angle to rotate about that axis.

The axis we want to use is the `Z` axis. In Unity there is a `Vector3` vector defined as the `Z` axis, and it is called `Vector3.forward`. The angle we want to use will depend on the hand we are moving. For the second hand we will want to move $\frac{1}{60}$ of a circle for each tick. And since a circle is $360\degree$, we need to rotate $\frac{360}{60}$ degrees.

Rather than performing the calculation for the computer, we can write the calculation into the code. This has the benefit of documenting the calculation and should help us if we ever need to come back to it later.

```cs
        float degreesPerSecond = 360 / 60;
        while (true)
        {
            secondHand.Rotate(Vector3.forward, degreesPerSecond);
            yield return new WaitForSeconds(1);
        }
```

Switch back to Unity and check that the second hand is turning one revolution per minute.

## Minutes and hours

At this point we have achieved the main objective of this tutorial, which is to implement a coroutine. But to finish things off, we can add in the minute and hour hands. We can move these with similar code, but with a different amount of degrees per tick. For the minute hand we want to rotate by $\frac{1}{60}$ of the second hand, and for the hour hand we want to rotate $\frac{1}{12}$ of that. Again, we can encode these values into the program:

```cs
        float degreesPerSecond = 360 / 60;
        float degreesPerMinute = degreesPerSecond / 60;
        float degreesPerHour = degreesPerMinute / 12;
```

Then for the rotation, we just need to rotate each hand by the calculated amount:

```cs
            secondHand.Rotate(Vector3.forward, degreesPerSecond);
            minuteHand.Rotate(Vector3.forward, degreesPerMinute);
            hourHand.Rotate(Vector3.forward, degreesPerHour);
```

Flip back to Unity and run to test. Your result should hopefully be something like what we were aiming for at the beginning.

https://github.com/user-attachments/assets/c280e06b-3ff0-4a04-a921-d0c41dc6eee8

You will be waiting a long time to test the hour hand, but if you go into the Unity settings, you can change the time scale to speed up time.

![the Time section of the Unity project settings window](https://github.com/user-attachments/assets/25c906b9-4310-4395-9579-acc100c790de)

## Tidy up

We can remove the unnecessary `using` instructions and empty functions as usual. In this case because we only have one coroutine and we are starting it once from the `Start` function, we can also embed the coroutine code into the `Start` function itself.

```cs
using System.Collections;
using UnityEngine;

public class Clock : MonoBehaviour
{
    public Transform secondHand;
    public Transform minuteHand;
    public Transform hourHand;

    IEnumerator Start()
    {
        float degreesPerSecond = 360 / 60;
        float degreesPerMinute = degreesPerSecond / 60;
        float degreesPerHour = degreesPerMinute / 12;
        while (true)
        {
            secondHand.Rotate(Vector3.forward, degreesPerSecond);
            minuteHand.Rotate(Vector3.forward, degreesPerMinute);
            hourHand.Rotate(Vector3.forward, degreesPerHour);
            yield return new WaitForSeconds(1);
        }
    }
}
```

## Accuracy

One thing you might notice with this code as it stands, is that it doesn't keep time very well. The accuracy problem here is that the `WaitForSeconds` function can only wait for a duration to within the nearest frame interval. This means the accuracy is somewhat framerate dependent. To correct for accuracy problems the coroutine would need to take the amount of time elapsed since the last update into account when setting the next wait.

This is left as an exercise for the reader. Good luck!