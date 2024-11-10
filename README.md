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

As a child of this object, add three empty children: `SecondHandPivot`, `MinuteHandPivot`, and `HourHandPivot`. Now we can add a cylinder as a fourth child, `FaceModel`, scaled to be a thin disc and rotated to face the camera. And as a child of each pivot add a cube to act as the respective hand model. Each should long, thin, and offset relative to the pivot.

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

