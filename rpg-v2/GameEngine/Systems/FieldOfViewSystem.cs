using System;
using System.Collections.Generic;
using System.Linq;
using game.GameEngine.Components;
using rpg_v2;

namespace game.GameEngine.Systems
{
    public static class FieldOfViewSystem
    {
        private static Dictionary<Position,Entity> Entities;
        public static void Act()
        {
            Entities = EcsManager.QueryEntitiesByComponentsIndexes(new[] {0, 3})
                .Where(x=>((Physics) x.Components[3]).BlocksVision)
                .ToDictionary(entity => entity.Components[0] as Position);

            foreach (var entity in EcsManager.QueryEntitiesByComponentsIndexes(new []{0,4}))
            {
                UpdateFieldOfView((Position) entity.Components[0],(Vision) entity.Components[4]);                
            }
            
            
            

        }
        
        public static void UpdateFieldOfView(Position position, Vision vision)
        {

            var xycomp = new[] {0, 1, -1, 0, 0, -1, 1, 0};
            var xxcomp = new[] {1, 0, 0, -1, -1, 0, 0, 1};
            var yxcomp = new[] {0, 1, 1, 0, 0, -1, -1, 0};
            var yycomp = new[] {1, 0, 0, 1, -1, 0, 0, -1};



            for (var i = 0; i < vision.CellsInLightOfSight.Length; i++)
            {
                for (var j = 0; j < vision.CellsInLightOfSight.Length; j++)
                {
                    vision.CellsInLightOfSight[i][j] = false;
                }
            }
            
            vision.CellsInLightOfSight[position.X][position.Y] = true;

            
            for (int i = 0; i < xxcomp.Length; i++)
            {
                Scan(0,position,1,0,xxcomp[i],yycomp[i],xycomp[i],yxcomp[i], vision);
            }

        }
        private static void Scan(int row,Position position,float startSlope,float endSlope,int xx,int yy,int xy,int yx, Vision vision)
        {
            var startX = position.X;
            var startY = position.Y;
            var blocked = false;
            for (int i = row; i < vision.Sight; i++)
            {
                var minCol = (int)Math.Round(i * startSlope);
                var maxCol = (int)Math.Round(i * endSlope);
                if (!blocked)
                {
                    for (int j = minCol; j >= maxCol; j--)
                    {
                        var deltaY = j * yy + i*yx;
                        var deltaX = i * xx + j*xy;


                        var doesBlockingEntityExist = Entities.ContainsKey(new Position(){X = deltaX+startX,Y = startY+deltaY});
                            
                        
                        if (j == minCol && doesBlockingEntityExist)
                        {
                            
                            vision.VisitedCells[startX + deltaX][startY + deltaY] = true;
                            vision.CellsInLightOfSight[startX + deltaX][startY + deltaY] = true;
                            
                            
                            blocked = true;
                            continue;
                        }
                        
                        if (!blocked && !doesBlockingEntityExist)
                        {
                            
                            vision.VisitedCells[startX + deltaX][startY + deltaY] = true;
                            vision.CellsInLightOfSight[startX + deltaX][startY + deltaY] = true;
                            
                        }
                        if (blocked && doesBlockingEntityExist)
                        {
                            
                            
                            vision.VisitedCells[startX + deltaX][startY + deltaY] = true;
                            vision.CellsInLightOfSight[startX + deltaX][startY + deltaY] = true;
                            
                            continue;
                        }
                        if (blocked && !doesBlockingEntityExist)
                        {
                            blocked = false;
                            
                            vision.VisitedCells[startX + deltaX][startY + deltaY] = true;
                            vision.CellsInLightOfSight[startX + deltaX][startY + deltaY] = true;
                            
                            startSlope = (j + 0.5f) / (i + 0.5f);
                        }
                        if (!blocked && doesBlockingEntityExist)
                        {

                            vision.VisitedCells[startX + deltaX][startY + deltaY] = true;
                            vision.CellsInLightOfSight[startX + deltaX][startY + deltaY] = true;
                            
                            var newEndSlope = (j + 0.5f) /(i - 0.5f);
                            Scan(i+1,position,startSlope,newEndSlope,xx,yy,xy,yx,vision);
                            blocked = true;
                        }
                    }
                }
            }
        }
        
        
    }
}