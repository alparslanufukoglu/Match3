using UnityEngine;

public class Tile
{
   public int posX;
   public int posY;
   public int tileType;
   public Sprite tileSprite;
   
   public Tile() {}

   public Tile(int posX, int posY, int tileType, Sprite tileSprite)
   {
      this.posX = posX;
      this.posY = posY;
      this.tileType = tileType;
      this.tileSprite = tileSprite;
   }
}
