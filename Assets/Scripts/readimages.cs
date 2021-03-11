using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class readimages : MonoBehaviour
{

    #region Private fields

    VoxelGrid _voxelGrid;

    //09 Create image variable
    Texture2D _sourceImage;

    #endregion

    #region Unity methods

    public Texture2D ImageFromFile(bool input = true) 
    {
        if (input)
        {
            _sourceImage = Resources.Load<Texture2D>("Data/1");
        }
        return _sourceImage;
    }

    public void Start()
    {
        
    

        


    }

    #endregion

    #region Public methods

    //38 Create public method to read image from button
    /// <summary>
    /// Read the image and set the states of the Voxels
    /// </summary>


    #endregion
}
