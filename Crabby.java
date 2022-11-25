package Entities;

import static utility.Constants.NPC.*;

import java.awt.Graphics;
import java.awt.image.BufferedImage;

import utility.LoadSave;
import utility.NPCID;

import utility.Vector2;

public class Crabby extends NPC {
    BufferedImage[] imgs;
    BufferedImage img;

    public Crabby(Vector2 position) {
        super(position, CRABBY_W, CRABBY_H, NPCID.CRABBY);
        img = getSprite(LoadSave.CRABBY_ATLAS);
        imgs = setLengthofAniSprite(1);
        imgs = Animation(imgs, img, 0, 0, CRABBY_W_DEF, CRABBY_H_DEF, true, false);
        type = NPCID.CRABBY;
    }

    @Override
    public void update() {
        updateHitbox();
        Move();
    }

    float speed = 0.5f;

    public void Move() {
        velocity = Vector2.Multiply(Vector2.SafeNormalize(Vector2.Substact(player.position, position)), speed);
        position = Vector2.Additive(velocity, position);
    }

    @Override
    public void draw(Graphics g, Vector2 lvloffset) {
        if (uniqueID == 20) {
            g.drawLine((int) (player.position.X - lvloffset.X), (int) (player.position.Y - lvloffset.Y),
                    (int) (position.X - lvloffset.X), (int) (position.Y - lvloffset.Y));
        }
        g.drawImage(imgs[0], (int) (position.X - lvloffset.X),
                (int) (position.Y - lvloffset.Y), CRABBY_W, CRABBY_H, null);
    }
}
