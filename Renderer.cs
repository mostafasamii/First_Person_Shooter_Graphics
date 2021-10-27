using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tao.OpenGl;
using GlmNet;
using System.IO;
using Graphics._3D_Models;
namespace Graphics
{
    class Renderer
    {
        Shader sh;
        Shader sh2;

        uint vertexBufferID;
        uint vertexBufferID2;

        int transID;
        int viewID;
        int projID;

        mat4 scaleMat;
        mat4 ProjectionMatrix;
        mat4 ViewMatrix;

        mat4 groundmatrix;
        mat4 backmatrix;
        mat4 upmatrix;
        mat4 rightmatrix;
        mat4 leftmatrix;

        public Camera cam;

        Texture tex;
        Texture down;
        Texture up;
        Texture right;
        Texture left;
        Texture front;
        Texture back;

        int EyePositionID;
        int AmbientLightID;
        int DataID;

  
        public float Speed = 1;

      

        int modelID;

     

        public md2LOL m;
        public md2LOL m2;
        Model3D tree;

        Model3D treee3;

        Model3D tree4;

        Model3D cottage;

        Model3D jeep;

        Model3D jeep2;

        //Model3D car;

    

        Texture tex1;

        mat4 modelmatrix;
        public void Initialize()
        {

            string projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            sh = new Shader(projectPath + "\\Shaders\\SimpleVertexShader.vertexshader", projectPath + "\\Shaders\\SimpleFragmentShader.fragmentshader");

            tex = new Texture(projectPath + "\\Textures\\crate.jpg", 0);
            down = new Texture(projectPath + "\\Textures\\down.jpg", 1);
            right = new Texture(projectPath + "\\Textures\\right.jpg", 2);
            left = new Texture(projectPath + "\\Textures\\left.jpg", 3);
            up = new Texture(projectPath + "\\Textures\\up.jpg", 4);
            front = new Texture(projectPath + "\\Textures\\front.jpg", 5);
            back = new Texture(projectPath + "\\Textures\\back.jpg", 6);
            tex1 = new Texture(projectPath + "\\Textures\\Ground.jpg", 2);

            m = new md2LOL(projectPath + "\\ModelFiles\\zombie.md2");
            m.StartAnimation(animType_LOL.RUN);
            m.rotationMatrix = glm.rotate((float)((-90.0f / 180) * Math.PI), new vec3(1, 0, 0));
            m.scaleMatrix = glm.scale(new mat4(1), new vec3(0.0093f, 0.0093f, 0.0093f));
            m.TranslationMatrix = glm.translate(new mat4(1), new vec3(0, -2, 5));
            ////define normal for each vertex
            m2 = new md2LOL(projectPath + "\\ModelFiles\\wolf.md2");
            m2.StartAnimation(animType_LOL.ATTACK1);
            m2.rotationMatrix = glm.rotate((float)((-90.0f / 180) * Math.PI), new vec3(1, 0, 0));
            m2.scaleMatrix = glm.scale(new mat4(1), new vec3(0.008f, 0.008f, 0.008f));
            m2.TranslationMatrix = glm.translate(new mat4(1), new vec3(-2, -2, 5));


            Gl.glClearColor(0, 0, 0, 1);
            float ground = 1;


            float[] skybox = {
                 -ground, -ground,- ground,   0,0,
                 ground, -ground, -ground,  1,0,
                 ground, ground, -ground,   1,1,

                 -ground, -ground, -ground,   0,0,
                -ground, ground, -ground,     0,1,
                 ground, ground, -ground,     1,1
            };
            float[] verts = {
                -1.0f, -1.0f, 0.0f,

                 0, 0,
                 1.0f, -1.0f, 0.0f,

                 1, 0,
                 0.0f,  1.0f, 0.0f,

                 0.5f, 1
            };

            vertexBufferID = GPU.GenerateBuffer(skybox);
            vertexBufferID2 = GPU.GenerateBuffer(verts);

            scaleMat = glm.scale(new mat4(1), new vec3(200f, 200f, 200f));

            cam = new Camera();

            ProjectionMatrix = cam.GetProjectionMatrix();
            ViewMatrix = cam.GetViewMatrix();
            transID = Gl.glGetUniformLocation(sh.ID, "model");
            projID = Gl.glGetUniformLocation(sh.ID, "projection");
            viewID = Gl.glGetUniformLocation(sh.ID, "view");

            leftmatrix = MathHelper.MultiplyMatrices(new List<mat4>() {glm.rotate(90.0f/180.0f*3.1412f,new vec3(0,1,0)),
                glm.translate(new mat4(1),new vec3(0,0,0)),
                scaleMat

            });

            rightmatrix = MathHelper.MultiplyMatrices(new List<mat4>() {glm.rotate(270f/180.0f*3.1412f,new vec3(0,1,0)),
                glm.translate(new mat4(1),new vec3(0,0,0)),
                scaleMat
            });

            upmatrix = MathHelper.MultiplyMatrices(new List<mat4>() {glm.rotate(90/180.0f*3.1412f,new vec3(1,0,0)),
                glm.translate(new mat4(1),new vec3(0,0,0)),
                scaleMat
            });

            backmatrix = MathHelper.MultiplyMatrices(new List<mat4>() {glm.rotate(180f/180f*3.1412f,new vec3(0,1,0)),
                glm.translate(new mat4(1),new vec3(0,0,0)),
                scaleMat
            });

            groundmatrix = MathHelper.MultiplyMatrices(new List<mat4>() {glm.rotate(90f/180.0f*3.1412f,new vec3(1,0,0)),
                glm.translate(new mat4(1),new vec3(0,-2,0)),
                scaleMat

            });

            //Gl.glClearColor(0, 0, 0.4f, 1);

            //cam = new Camera();
           cam.Reset(0, -1.75f, 0, 0, 0, 0, 0, 0, 0);

            ProjectionMatrix = cam.GetProjectionMatrix();
            ViewMatrix = cam.GetViewMatrix();

            

            modelmatrix = glm.scale(new mat4(1), new vec3(50, 50, 50));

            tree = new Model3D();
            tree.LoadFile(projectPath + "\\ModelFiles\\tree", 6, "Tree.obj");
            tree.scalematrix = glm.scale(new mat4(1), new vec3(0.1f, 0.1f, 0.1f));
            tree.transmatrix = glm.translate(new mat4(1), new vec3(0.6f, -2, 0.4f));

            //tree2 = new Model3D();
            //tree2.LoadFile(projectPath + "\\ModelFiles\\Tree1", 6, "Tree1.obj");
            //tree2.scalematrix = glm.scale(new mat4(1), new vec3(30, 30, 30));

            treee3 = new Model3D();
            treee3.LoadFile(projectPath + "\\ModelFiles\\Wolf Rigged and Game Ready", 6, "Wolf.fbx");
            treee3.scalematrix = glm.scale(new mat4(1), new vec3(0.1f, 0.1f, 0.1f));
            treee3.transmatrix = glm.translate(new mat4(1), new vec3(-0.6f, -2, 0.4f));
            treee3.rotmatrix = glm.rotate((float)((-180.0f / 180) * Math.PI), new vec3(1, 0, 0));


            //car = new Model3D();
            //car.LoadFile(projectPath + "\\ModelFiles\\car", 6, "dpv.obj");
            //car.scalematrix = glm.scale(new mat4(1), new vec3(15, 1, 15));

            cottage = new Model3D();
            cottage.LoadFile(projectPath + "\\ModelFiles\\House", 6, "house.obj");
            cottage.scalematrix = glm.scale(new mat4(1), new vec3(0.1f, 0.1f, 0.1f));

            cottage.transmatrix = glm.translate(new mat4(1), new vec3(-0.6f, -2f, 0.4f));

            jeep = new Model3D();
            jeep.LoadFile(projectPath + "\\ModelFiles\\jeep", 9, "jeep1.3ds");
            jeep.scalematrix = glm.scale(new mat4(1), new vec3(0.05f, 0.05f,
               0.05f));
            jeep.transmatrix = glm.translate(new mat4(1), new vec3(1f, -2f, 1f));
            jeep.rotmatrix = glm.rotate((float)((-90.0f / 180) * Math.PI), new vec3(1, 0, 0));

            //jeep2 = new Model3D();
            //jeep2.LoadFile(projectPath + "\\ModelFiles\\jeep", 10, "jeep1.3ds");
            //jeep2.scalematrix = glm.scale(new mat4(1), new vec3(5, 5, 5));
            //jeep2.transmatrix = glm.translate(new mat4(1), new vec3(-80, 1, 180));
            //jeep2.rotmatrix = glm.rotate((float)((-90.0f / 180) * Math.PI), new vec3(1, 0, 0));

            //get location of specular and attenuation then send their values
            //get location of light position and send its value
            //setup the ambient light component.
            //setup the eye position.

            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glDepthFunc(Gl.GL_LESS);
        }

        public void Draw()
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT|Gl.GL_DEPTH_BUFFER_BIT);
            sh.UseShader();
            mat4 nv = new mat4(ViewMatrix[0], ViewMatrix[1], ViewMatrix[2], ViewMatrix[3]);
            nv[3, 0] = 0;
            nv[3, 1] = 0;
            nv[3, 2] = 0;
            Gl.glUniformMatrix4fv(transID, 1, Gl.GL_FALSE, scaleMat.to_array());
            Gl.glUniformMatrix4fv(projID, 1, Gl.GL_FALSE, ProjectionMatrix.to_array());
            Gl.glUniformMatrix4fv(viewID, 1, Gl.GL_FALSE, nv.to_array());


            Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, vertexBufferID);
            Gl.glEnableVertexAttribArray(0);
            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 5 * sizeof(float), IntPtr.Zero);

            Gl.glEnableVertexAttribArray(2);
            Gl.glVertexAttribPointer(2, 2, Gl.GL_FLOAT, Gl.GL_FALSE, 5 * sizeof(float), (IntPtr)(3 * sizeof(float)));
            //Gl.glDepthMask(0);
            front.Bind();
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, 6);

            left.Bind();
            Gl.glUniformMatrix4fv(transID, 1, Gl.GL_FALSE, leftmatrix.to_array());
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, 6);
            right.Bind();
            Gl.glUniformMatrix4fv(transID, 1, Gl.GL_FALSE, rightmatrix.to_array());
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, 6);
            down.Bind();
            Gl.glUniformMatrix4fv(transID, 1, Gl.GL_FALSE, groundmatrix.to_array());
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, 6);
            up.Bind();
            Gl.glUniformMatrix4fv(transID, 1, Gl.GL_FALSE, upmatrix.to_array());
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, 6);
            back.Bind();
            Gl.glUniformMatrix4fv(transID, 1, Gl.GL_FALSE, backmatrix.to_array());
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, 6);

            //sh.UseShader();
            //Update(0);
            //Gl.glUniformMatrix4fv(transID, 1, Gl.GL_FALSE, scaleMat.to_array());
            //Gl.glUniformMatrix4fv(projID, 1, Gl.GL_FALSE, ProjectionMatrix.to_array());
            //Gl.glUniformMatrix4fv(viewID, 1, Gl.GL_FALSE, ViewMatrix.to_array());
            //Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, vertexBufferID2);
            //Gl.glEnableVertexAttribArray(0);
            //Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 5 * sizeof(float), IntPtr.Zero);

            //Gl.glEnableVertexAttribArray(1);
            //Gl.glVertexAttribPointer(1, 2, Gl.GL_FLOAT, Gl.GL_FALSE, 5 * sizeof(float), (IntPtr)(3 * sizeof(float)));
            //tex.Bind();
            //Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, 3);
            //send the value of camera position (eye position)
            //Update(0.5f);
            //sh.UseShader();
      
           // Gl.glUniformMatrix4fv(projID, 1, Gl.GL_FALSE, ProjectionMatrix.to_array());

            Gl.glUniformMatrix4fv(viewID, 1, Gl.GL_FALSE, ViewMatrix.to_array());
            m.Draw(transID);
            tree.Draw(transID);
            ////car.Draw(transID);
            cottage.Draw(transID);
            //// tree2.Draw(transID);
            treee3.Draw(transID);
            jeep.Draw(transID);
            //jeep2.Draw(transID);
            m2.Draw(transID);
           // Gl.glUniformMatrix4fv(viewID, 1, Gl.GL_FALSE, ViewMatrix.to_array());
            //Gl.glUniformMatrix4fv(projID, 1, Gl.GL_FALSE, ProjectionMatrix.to_array());



            m.AnimationSpeed = 0.01f;

          //  Gl.glUniformMatrix4fv(viewID, 1, Gl.GL_FALSE, ViewMatrix.to_array());
           // Gl.glUniformMatrix4fv(transID, 1, Gl.GL_FALSE, scaleMat.to_array());
            Gl.glDisableVertexAttribArray(0);
            Gl.glDisableVertexAttribArray(1);
            Gl.glDisableVertexAttribArray(2);
        

    }
        public void Update(float deltaTime)
        {
            cam.UpdateViewMatrix();
            ProjectionMatrix = cam.GetProjectionMatrix();
            ViewMatrix = cam.GetViewMatrix();
            m.UpdateExportedAnimation();
        }
        //public void Update()
        //{
        //    cam.UpdateViewMatrix();
        //   ProjectionMatrix = cam.GetProjectionMatrix();
        //    ViewMatrix = cam.GetViewMatrix();
        //    m.UpdateExportedAnimation();
        //}
        public void SendLightData(float red, float green, float blue, float attenuation, float specularExponent)
        {
            vec3 ambientLight = new vec3(red, green, blue);
            Gl.glUniform3fv(AmbientLightID, 1, ambientLight.to_array());
            vec2 data = new vec2(attenuation, specularExponent);
            Gl.glUniform2fv(DataID, 1, data.to_array());
        }
        public void CleanUp()
        {
            sh.DestroyShader();
        }
    }
}
