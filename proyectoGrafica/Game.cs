using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyectoGrafica
{
    public class Game : GameWindow

    {
        int VertexBufferObject;
        int indexBufferHandle;
        private int vertexArrayObject;
        float[] vertices = {
        // Rectángulo principal (Front face)
        -0.5f,  0.5f,  0.2f,  1.0f, 0.0f, 0.0f, // Red
         0.5f,  0.5f,  0.2f,  1.0f, 0.0f, 0.0f, // Red
         0.5f, -0.5f,  0.2f,  1.0f, 0.0f, 0.0f, // Red
        -0.5f, -0.5f,  0.2f,  1.0f, 0.0f, 0.0f, // Red

        // Back face
        -0.5f,  0.5f, -0.2f,  0.0f, 1.0f, 0.0f, // Green
         0.5f,  0.5f, -0.2f,  0.0f, 1.0f, 0.0f, // Green
         0.5f, -0.5f, -0.2f,  0.0f, 1.0f, 0.0f, // Green
        -0.5f, -0.5f, -0.2f,  0.0f, 1.0f, 0.0f, // Green

        // Soporte en forma de T (Front)
        -0.2f,  -0.55f,  0.2f,  0.0f, 0.0f, 1.0f, // Blue 
         0.2f,  -0.55f,  0.2f,  0.0f, 0.0f, 1.0f, // Blue 
         0.2f,  -0.6f,  0.2f,   0.0f, 0.0f, 1.0f, // Blue 
        -0.2f,  -0.6f,  0.2f,   0.0f, 0.0f, 1.0f, // Blue 

        // Soporte en forma de T (Back)
        -0.2f,  -0.55f, -0.2f,  0.0f, 0.0f, 1.0f, // Blue 
         0.2f,  -0.55f, -0.2f,  0.0f, 0.0f, 1.0f, // Blue 
         0.2f,  -0.6f,  -0.2f,  0.0f, 0.0f, 1.0f, // Blue 
        -0.2f,  -0.6f,  -0.2f,  0.0f, 0.0f, 1.0f  // Blue 
        };

        int[] indices = {
        // Soporte en forma de T
        8, 9, 10,  8, 10, 11,
        12,13,14, 12,14,15,
        12,8,11, 12,11,15,
        13,9,10, 13,10,14,
                
        13,9,8, 13,8,12,      
    
        // Rectángulo principal
        0, 1, 2,  0, 2, 3,
        // Rectángulo inferior
        4, 5, 6,  4, 6, 7,
        // Caras laterales
        0, 1, 5,  0, 5, 4,
        3, 2, 6,  3, 6, 7,
        // Caras superiores e inferiores
        0, 3, 7,  0, 7, 4,
        1, 2, 6,  1, 6, 5,

        };

        Shader shader;
        
        public Game(int width, int height, string title) : base(width, height, GraphicsMode.Default, title) { 
        
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
             
            KeyboardState input = Keyboard.GetState();  

            if (input.IsKeyDown(Key.Escape)) //para la ventanda se cierre con el escape
            {
                Exit();
            }
        }

        //Cualquier código relacionado con la inicialización debe ir aquí.
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Configurar VAO, VBO y EBO
            int VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            int indexBufferHandle = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBufferHandle);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(int), indices, BufferUsageHint.StaticDraw);

            vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayObject);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBufferHandle);

            // Posiciones
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // Colores
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);


            // Usar el shader
            shader = new Shader(@"D:\materias\grafica\proyecto\proyectoGrafica\proyectoGrafica\shader\shader.vert", @"D:\materias\grafica\proyecto\proyectoGrafica\proyectoGrafica\shader\shader.frag");
            shader.Use();

            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), (float)Width / Height, 0.1f, 100f);
            Matrix4 view = Matrix4.LookAt(new Vector3(3, 0, 3), Vector3.Zero, Vector3.UnitY); // Cámara en el lado derecho mirando al centro

            shader.SetProjectionMatrix(projection);
            shader.SetViewMatrix(view);

        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.BindVertexArray(vertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);

            SwapBuffers();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Width, Height);
        }

        protected override void OnUnload(EventArgs e)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            // Delete all the resources.
            GL.DeleteBuffer(VertexBufferObject);
            GL.DeleteVertexArray(vertexArrayObject);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer,0);
            GL.DeleteBuffer(this.indexBufferHandle);

            base.OnUnload(e);
            shader.Dispose();
        }
    }
}
