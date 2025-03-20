---
date: 2025-01-04T16:00:00
tags:
  - GameDev
  - GraphicsProgramming
  - Shaders
  - General
---
***
>Ces notes sont des notes prises en suivant la [Vidéo](https://youtu.be/BrZ4pWwkpto) de Game Dev Guide sur l'introduction au Compute Shaders.
***
# Compute Buffers
### Intro & Structures
> Un Compute Shader, écrit en HLSL est généralement (ou dans le cas de Unity) structuré comme tel :
```
﻿#pragma kernel CSMain // Déclare la fonction qui sera éxécutée par le Compute Shader, nomée le Kernel

struct ValueStruct {

float3 num;

// Dans ce struct seront déclarées toutes les variables, généralement float3, float4 et int, qui seront uttilisées par le Shader
// Les valeurs de ses variables seront ensuites remplies par le Compute Buffer
};

RWStructureBuffer<ValueStruct> values;
// Un Read Write Structure Buffer, instancie la struct et permet de faire communiquer le Shader et le Buffer, commpe par exemple écrire les valeurs résultantes de la Simulation

[numthreads(1,1,1)]
//COMPLIQUE

void CSMain(unit3 id : SV_DispatchThreadID) {
//id est le marqueur de nos threads, on le référencera afin d'obtenir nos objets individuels à simuler
// Ici la fonction prinicpale sera executée, généralement de la facon suivante

ValueStruct localvalue = values[id.x] 
// Data fetch
localvalue.num += 55555;
// Data compute
values[id.x] = localvalue
//Data set
}
```

> Un Compute Buffer, est lui écrit en C# et est généralement (ou dans le cas de Unity) structuré comme tel, et est compris dans la classe qui appelle le Shader :
 
```
public struct ValueStruct {
 public Vector3 num;
}

public ComputeShader computeShader
// A assigner dans l'inspecteur

// Le Struct contenant les informations à passer au Shader

private ValuesStruct[] Data;

// Un array de nos structs, à peupler des valeurs de nos objets individuels, pour des besoins de lisibilité, le code ne sera pas montré (boucle for lorsqu'on instancie nos objets)

private void CallCompute() {

// La Méthode qui apellera notre Compute Buffer

ComputeBuffer objetcsBuffer = new ComputeBuffer(Data.Length, sizeOf(float)*3)

//Le Compute Buffer doit avoir en paramètres la taille de notre array de strucs (notre nombre d'objets) et la taille d'un struct individuel en bytes (la somme de nos variables individuelles, ici juste num, un Vector3, obtenable via sizeOf(float)*3 (Le Vector3 etant composé de 3 floats))

objetcsBuffer.SetData(Data); 

//On set les valeurs de notre Buffer par notre struct

computeShader.SetBuffer(0, "values", objetcsBuffer);
//Ici le string est le nom donné à l'instance de notre struct nomée dans le Shader (RWStructureBuffer)

computeSHader.Dispatch(0, Data.Length / 10,1,1);
// mentionner la taille de l'array ainsi que le nombre de threads

objetcsBuffer.GetData(Data);
//On laisse le Shader Réécrire nos structs

//Apres le dispatch, Set la nouvelle Data dans nos objets, via une boucle for
//
//
//

objetcsBuffer.Dispose();
//On lache le buffer apres le compute
}


```
### Buffers et Shaders
- Les Compute Buffers sont utilisés pour permettre une communication efficace entre le Processeur et la Carte Graphique, sur lequel le Compute Shader est exécuté en parallèle.
- Les Compute Shaders emploient les Calculs en Parallèle et sont ainsi plus efficaces que le CPU dans des cas de Simulation et de Génération d'Images et de calculs répétitifs.
### Mise en Place
- Dans notre cas, un Compute Shader doit avoir accès au données nécessaires pour la simulation, qui lui seront fournies via le Compute Buffer, généralement dans un Struct.
