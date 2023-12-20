// Ce script doit être placé dans le dossier Assets>Plugins d'un projet Unity.
// Ensuite, pour utiliser la fonction SynchroniserWebGL à partir d'une classe C#, il faut y ajouter:
//     using System.Runtime.InteropServices;
//         et 
//     [DllImport("__Internal")] 
//     private static extern void SynchroniserWebGL();

// Ce script est basé sur le code de Johan Rensenbrink:
// https://forum.unity.com/threads/how-does-saving-work-in-webgl.390385/#post-3689683

var SauvegardeWebGL = {
     SynchroniserWebGL : function()
     {
         var GererErreur = function (err) {if(err!=null){console.log("Une erreur de synchro est survenue. "+err);}}
         FS.syncfs(false, GererErreur);
     }
};
mergeInto(LibraryManager.library, SauvegardeWebGL);