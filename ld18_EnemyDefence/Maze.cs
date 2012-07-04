using System;
using System.Collections;
using System.Collections.Generic;

/*
    A dfs  
*/

namespace ld18_EnemyDefence
{    
    public class Maze
    {
        public static int kDimension = GameTypes.MAP_WIDTH/2;
        Cell[,] Cells = null;
        Stack CellStack = new Stack();
        int VisitedCells = 1;
        int TotalCells = kDimension * kDimension;
        Cell CurrentCell = null;

        public int[,] getWalkableArray()
        {
            int[,] returnList = new int[GameTypes.MAP_WIDTH,GameTypes.MAP_WIDTH];

            int topleft = 0;
            int topright = 0;
            int bottomleft = 0;
            int bottomright = 0;

            int yoffset = 1;

            //walkable 0, unwalkable =1

            for (int i = 0; i < kDimension * kDimension; i++)
            {
                topleft = 1;
                topright = 1;
                bottomleft = 1;
                bottomright = 1;

                //Do the west and south walls.
                //west
                if (Cells[i % kDimension, i / kDimension].Walls[1] == 1)
                {
                    topleft = 0;
                    bottomleft = 0;
                }
                if (Cells[i % kDimension, i / kDimension].Walls[2] == 1)
                {
                    bottomleft = 0;
                    bottomright = 0;
                }

                // maze x = map x*2

                returnList[(i % kDimension) * 2,(i / kDimension) * 2 + yoffset]=topleft;
                returnList[((i % kDimension) * 2)+1,(i / kDimension) * 2 + yoffset] = topright;
                returnList[((i % kDimension) * 2)+1,((i / kDimension) * 2) +1 + yoffset] = bottomright;
                returnList[(i % kDimension) * 2,((i / kDimension) * 2)+1 + yoffset] = bottomleft;                

                //if a square has both a right and up wall, fill the corner.
                if (i / kDimension > 0 && i % kDimension < kDimension - 1)
                {
                    if (Cells[i % kDimension, i / kDimension].Walls[0] == 1
                        && Cells[i % kDimension, i / kDimension].Walls[3] == 1)
                    {                        
                        returnList[((i % kDimension) * 2)+2,((i / kDimension) * 2) - 1 + yoffset] = 1; 
                    }
                }
            }

            return returnList;
        }

        public Maze()
        {          
            Initialize();
        }


        private ArrayList GetNeighborsWithWalls(Cell aCell)
        {
            ArrayList Neighbors = new ArrayList();
            int count = 0;
            for (int countRow = -1; countRow <= 1; countRow++)
                for (int countCol = -1; countCol <= 1; countCol++)
                {
                    if ((aCell.Row + countRow < kDimension) &&
                         (aCell.Column + countCol < kDimension) &&
                         (aCell.Row + countRow >= 0) &&
                         (aCell.Column + countCol >= 0) &&
                         ((countCol == 0) || (countRow == 0)) &&
                         (countRow != countCol)
                        )
                    {
                        if (Cells[aCell.Row + countRow, aCell.Column + countCol].HasAllWalls())
                        {
                            Neighbors.Add(Cells[aCell.Row + countRow, aCell.Column + countCol]);
                        }
                    }
                }

            return Neighbors;
        }

        public void Initialize()
        {
            Cells = new Cell[kDimension, kDimension];
            TotalCells = kDimension * kDimension;
            for (int i = 0; i < kDimension; i++)
                for (int j = 0; j < kDimension; j++)
                {
                    Cells[i, j] = new Cell();
                    Cells[i, j].Row = i;
                    Cells[i, j].Column = j;
                }

            CurrentCell = Cells[0, 0];
            VisitedCells = 1;
            CellStack.Clear();
        }

        public void Generate()
        {
            while (VisitedCells < TotalCells)
            {               
                ArrayList AdjacentCells = GetNeighborsWithWalls(CurrentCell);                
                if (AdjacentCells.Count > 0)
                {                   
                    int randomCell = Cell.TheRandom.Next(0, AdjacentCells.Count);
                    Cell theCell = ((Cell)AdjacentCells[randomCell]);
                    CurrentCell.KnockDownWall(theCell);
                    CellStack.Push(CurrentCell);
                    CurrentCell = theCell; 
                    VisitedCells++;
                }
                else
                {                    
                    CurrentCell = (Cell)CellStack.Pop();
                }

            }
        }
    }
}
