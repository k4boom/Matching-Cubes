using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class Extension
{
    public static bool isSameMaterial(this Transform one , Transform two) {
        return one.GetComponent<MeshRenderer>().sharedMaterial.name == two.GetComponent<MeshRenderer>().sharedMaterial.name;
    }

    //Matching is done through the Material Name
    public static bool isSameMaterial2(this Transform one , Transform two, Transform three) {
        return (one.GetComponent<MeshRenderer>().sharedMaterial.name == two.GetComponent<MeshRenderer>().sharedMaterial.name) &&
                (one.GetComponent<MeshRenderer>().sharedMaterial.name == three.GetComponent<MeshRenderer>().sharedMaterial.name);
    }

    //Fisher Yates Algorith for Random Gate
    public static List<Transform> Fisher_Yates_CardDeck_Shuffle (this List<Transform> aList) {
 
         System.Random _random = new System.Random ();
 
         Transform myGO;
 
         int n = aList.Count;
         for (int i = 0; i < n; i++)
         {
             // NextDouble returns a random number between 0 and 1.
             // ... It is equivalent to Math.random() in Java.
             int r = i + (int)(_random.NextDouble() * (n - i));
             myGO = aList[r];
             aList[r] = aList[i];
             aList[i] = myGO;
         }
 
         return aList;
    }
    //Write down the numbers from 1 through N.
    //Pick a random number k between one and the number of unstruck numbers remaining (inclusive).
    //Counting from the low end, strike out the kth number not yet struck out, and write it down at the end of a separate list.
    //Repeat from step 2 until all the numbers have been struck out.
    //The sequence of numbers written down in step 3 is now a random permutation of the original numbers.
}
