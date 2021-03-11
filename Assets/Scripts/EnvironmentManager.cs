using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.Linq;

public class EnvironmentManager : MonoBehaviour
{
    #region Fields and properties

    VoxelGrid _voxelGrid;
    int _randomSeed = 666;

    bool _showVoids = true;

    // 45 Create the inference object variable
    Pix2Pix _pix2pix;

    
    #endregion


    #region Unity Standard Methods

    void Start()
    {
        // Initialise the voxel grid
        Vector3Int gridSize = new Vector3Int(64,5, 64);
        _voxelGrid = new VoxelGrid(gridSize, Vector3.zero, 1, parent: this.transform);

        // Set the random engine's seed
        Random.InitState(_randomSeed);

        // 46 Create the Pix2Pix inference object
        _pix2pix = new Pix2Pix();

        
    }
    
    void Update()
    {

        DrawVoxels();
        // Use the V key to switch between showing voids
        if (Input.GetKeyDown(KeyCode.V))
        {
            _showVoids = !_showVoids;
        }

       
        // 11 Use the R key to clear the grid
        if (Input.GetKeyDown(KeyCode.R))
        {
            // 13 Clear the grid
            _voxelGrid.ClearGrid();
        }

        PredictAndUpdate(allLayers: true);

    }

    #endregion

    #region Private Methods

    // 47 Create the method to run predictions on the grid and update
    /// <summary>
    /// Runs the predictioin model on the grid
    /// </summary>
    /// <param name="allLayers">If it should run on all layers. Default is only layer 0</param>
    void PredictAndUpdate(bool allLayers = false)
    {
      
        // 50 Define how many layers will be used based on bool
        int layerCount = 1;
        if (allLayers) layerCount = _voxelGrid.GridSize.y;

        // 51 Iterate through all layers, running the model
        for (int i = 0; i < layerCount; i++)
        {
            // 52 Get the image from the grid's layer
             var _sourceImage = _voxelGrid.ImageFromFile();
            
            // 53 Resize the image to 256x256
            ImageReadWrite.Resize256(_sourceImage, Color.grey);

            // 54 Run the inference model on the image
            var image = _pix2pix.Predict(_sourceImage);

            // 56 Scale the image back to the grid size
            TextureScale.Point(image, _voxelGrid.GridSize.x, _voxelGrid.GridSize.z);

            // 57 Set the layer's voxels' states to the grid
            _voxelGrid.SetStatesFromImage(image, layer: i);
        }
    }
    void DrawVoxels()
    {
        foreach (var voxel in _voxelGrid.Voxels)
        {
            if (voxel.IsActive)
            {
                Vector3 pos = (Vector3)voxel.Index * _voxelGrid.VoxelSize + transform.position;
                if (voxel.FColor == FunctionColor.Black) Drawing.DrawCube(pos, _voxelGrid.VoxelSize, Color.black);
                else if (voxel.FColor == FunctionColor.Red) Drawing.DrawCube(pos, _voxelGrid.VoxelSize, Color.red);
                else if (voxel.FColor == FunctionColor.Yellow) Drawing.DrawCube(pos, _voxelGrid.VoxelSize, Color.yellow);
                else if (voxel.FColor == FunctionColor.Green) Drawing.DrawCube(pos, _voxelGrid.VoxelSize, Color.green);
                else if (voxel.FColor == FunctionColor.Cyan) Drawing.DrawCube(pos, _voxelGrid.VoxelSize, Color.cyan);
                else if (voxel.FColor == FunctionColor.Magenta) Drawing.DrawCube(pos, _voxelGrid.VoxelSize, Color.magenta);
                else if (_showVoids && voxel.Index.y == 0)
                    Drawing.DrawTransparentCube(pos, _voxelGrid.VoxelSize);
            }
        }
    }


    #endregion
}
