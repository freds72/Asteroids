import bpy
import random

def asteroid():
    bpy.ops.mesh.primitive_ico_sphere_add()
    bpy.ops.object.mode_set(mode='EDIT')
    bpy.ops.mesh.faces_shade_smooth()
    bpy.ops.mesh.select_mode(type='VERT')
    bpy.ops.mesh.select_all(action='DESELECT')
    bpy.ops.mesh.select_random()
    bpy.ops.transform.resize(value=(random.uniform(1.1,1.4),random.uniform(1.1,1.4),random.uniform(1.1,1.4)))
    bpy.ops.mesh.select_all(action='SELECT')
    bpy.ops.mesh.subdivide(smoothness=1)
    bpy.ops.mesh.select_all(action='DESELECT')
    bpy.ops.mesh.select_random()
    bpy.ops.transform.resize(value=(random.uniform(1.05,1.15),random.uniform(1.05,1.15),random.uniform(1.05,1.15)))
    bpy.ops.mesh.select_all(action='SELECT')
    bpy.ops.mesh.subdivide(smoothness=1)
    bpy.ops.mesh.select_all(action='DESELECT')
    bpy.ops.mesh.select_random()
    bpy.ops.transform.resize(value=(random.uniform(0.92,1.05),random.uniform(0.92,1.05),random.uniform(0.92,1.05)))
    bpy.ops.mesh.select_all(action='SELECT')
    bpy.ops.mesh.subdivide(smoothness=1)
    stretch = random.uniform(0.9,1.5)
    bpy.ops.transform.resize(value=(stretch,1,1))
    bpy.ops.transform.rotate(value=(random.uniform(-1.57,1.57)), axis=(random.uniform(-1.57,1.57),random.uniform(-1.57,1.57),random.uniform(-1.57,1.57)))
    bpy.ops.object.mode_set(mode='OBJECT')

asteroid()