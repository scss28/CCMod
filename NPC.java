package Entities;

import java.awt.Graphics;

import utility.Vector2;

public abstract class NPC extends entity {
    public int type = 0;
    public int uniqueID = 0;
    public boolean active = false;
    public Player player;

    public NPC(Vector2 position, int width, int height, int npcType) {
        super(position, width, height);
        this.type = npcType;
    }

    public void update() {
    }

    public void draw(Graphics g) {
    }

    public void draw(Graphics g, Vector2 lvloffset) {
    }
}
