﻿// See https://aka.ms/new-console-template for more information
// Tetris?? you serious?
// Update from day 2, i can't say it was easy, but it sertainly was fun and 
// Update from day 3, it was very fun to make and i'm extremely pround of what i've done
// Ukaszoro made this

using System.Runtime.Intrinsics.X86;
using SDL2;

public static class ArrayExtensions
{
    public static T[,] Extract2DArray<T>(this T[,,] array3d, int index = 0)
    {
        var dim0Length = array3d.GetLength(0);
        if (index < 0 || index >= dim0Length)
            throw new ArgumentOutOfRangeException(nameof(index), index, $"The index to extract from the 3-d array is out of range. Must be in the range [{0}..{dim0Length - 1}]");

        var dim1Length = array3d.GetLength(1);
        var dim2Length = array3d.GetLength(2);
        T[,] array2d = new T[dim1Length, dim2Length];
        for (int i = 0; i < dim1Length; i++)
            for (int j = 0; j < dim2Length; j++)
                array2d[i, j] = array3d[index, i, j];

        return array2d;
    }
}

interface Tetrominos
{
    public int[,] Shape { get; set; }
    public int Shape_current { get; set; }
    public int TopLeft_row { get; set; }
    public int TopLeft_col { get; set; }
    public int[,,] Rotations { get; set; }
}
class Square : Tetrominos
{
    public int[,] Shape { get; set; }
    public int Shape_current { get; set; }
    public int TopLeft_row { get; set; }
    public int TopLeft_col { get; set; }
    public int[,,] Rotations { get; set; }
    
    public Square(int c)
    {
        this.TopLeft_row = 0;
        this.TopLeft_col = 4;
        this.Rotations = new int[4, 2, 2] 
        {
            { { c, c }, 
              { c, c } },

            { { c, c }, 
              { c, c } },

            { { c, c }, 
              { c, c } },

            { { c, c },
              { c, c } }
        };
        this.Shape_current = 0;
        this.Shape = Rotations.Extract2DArray(this.Shape_current);
    }
}
class L : Tetrominos
{
    public int[,] Shape { get; set; }
    public int Shape_current { get; set; }
    public int TopLeft_row { get; set; }
    public int TopLeft_col { get; set; }
    public int[,,] Rotations { get; set; }
    
    public L(int c)
    {
        this.TopLeft_row = 0;
        this.TopLeft_col = 3;
        this.Rotations = new int[4, 3, 3] 
        {
            { {0, 0, 0 }, 
              {c, c, c } ,
              {c, 0, 0 } },

            { {c, c, 0 }, 
              {0, c, 0 } ,
              {0, c, 0 } },
            
            { {0, 0, c }, 
              {c, c, c } ,
              {0, 0, 0 } },

            { {0, c, 0 }, 
              {0, c, 0 } ,
              {0, c, c } },

        };
        this.Shape_current = 0;
        this.Shape = Rotations.Extract2DArray(this.Shape_current);
    }
}
class J : Tetrominos
{
    public int[,] Shape { get; set; }
    public int Shape_current { get; set; }
    public int TopLeft_row { get; set; }
    public int TopLeft_col { get; set; }
    public int[,,] Rotations { get; set; }
    
    public J(int c)
    {
        this.TopLeft_row = 0;
        this.TopLeft_col = 3;
        this.Rotations = new int[4, 3, 3] 
        {
            { {c, 0, 0 }, 
              {c, c, c } ,
              {0, 0, 0 } },

            { {0, c, 0 }, 
              {0, c, 0 } ,
              {c, c, 0 } },

            { {c, 0, 0 }, 
              {c, c, c } ,
              {0, 0, 0 } },

            { {0, c, c }, 
              {0, c, 0 } ,
              {0, c, 0 } },

        };
        this.Shape_current = 0;
        this.Shape = Rotations.Extract2DArray(this.Shape_current);
    }
}
class Triangle : Tetrominos
{
    public int[,] Shape { get; set; }
    public int Shape_current { get; set; }
    public int TopLeft_row { get; set; }
    public int TopLeft_col { get; set; }
    public int[,,] Rotations { get; set; }
    
    public Triangle(int c)
    {
        this.TopLeft_row = 0;
        this.TopLeft_col = 3;
        this.Rotations = new int[4, 3, 3] 
        {
            { {0, c, 0 }, 
              {c, c, c } ,
              {0, 0, 0 } },

            { {0, c, 0 }, 
              {0, c, c } ,
              {0, c, 0 } },

            { {0, 0, 0 }, 
              {c, c, c } ,
              {0, c, 0 } },

            { {0, c, 0 }, 
              {c, c, 0 } ,
              {0, c, 0 } },

        };
        this.Shape_current = 0;
        this.Shape = Rotations.Extract2DArray(this.Shape_current);
    }
}
class Z : Tetrominos
{
    public int[,] Shape { get; set; }
    public int Shape_current { get; set; }
    public int TopLeft_row { get; set; }
    public int TopLeft_col { get; set; }
    public int[,,] Rotations { get; set; }
    
    public Z(int c)
    {
        this.TopLeft_row = 0;
        this.TopLeft_col = 3;
        this.Rotations = new int[4, 3, 3] 
        {
            { {c, c, 0 }, 
              {0, c, c } ,
              {0, 0, 0 } },

            { {0, 0, c }, 
              {0, c, c } ,
              {0, c, 0 } },

            { {c, c, 0 }, 
              {0, c, c } ,
              {0, 0, 0 } },

            { {0, 0, c }, 
              {0, c, c } ,
              {0, c, 0 } },

        };
        this.Shape_current = 0;
        this.Shape = Rotations.Extract2DArray(this.Shape_current);
    }
}
class S : Tetrominos
{
    public int[,] Shape { get; set; }
    public int Shape_current { get; set; }
    public int TopLeft_row { get; set; }
    public int TopLeft_col { get; set; }
    public int[,,] Rotations { get; set; }
    
    public S(int c)
    {
        this.TopLeft_row = 0;
        this.TopLeft_col = 3;
        this.Rotations = new int[4, 3, 3] 
        {
            { {0, c, c }, 
              {c, c, 0 } ,
              {0, 0, 0 } },

            { {0, c, 0 }, 
              {0, c, c } ,
              {0, 0, c } },

            { {0, c, c }, 
              {c, c, 0 } ,
              {0, 0, 0 } },

            { {0, c, 0 }, 
              {0, c, c } ,
              {0, 0, c } },

        };
        this.Shape_current = 0;
        this.Shape = Rotations.Extract2DArray(this.Shape_current);
    }
}
class Line : Tetrominos
{
    public int[,] Shape { get; set; }
    public int Shape_current { get; set; }
    public int TopLeft_row { get; set; }
    public int TopLeft_col { get; set; }
    public int[,,] Rotations { get; set; }
    
    public Line(int c)
    {
        this.TopLeft_row = 0;
        this.TopLeft_col = 3;
        this.Rotations = new int[4, 4, 4] 
        {
            { {0, c, 0, 0 }, 
              {0, c, 0, 0 } ,
              {0, c, 0, 0 } ,
              {0, c, 0, 0 } },

            { {0, 0, 0, 0 }, 
              {c, c, c, c } ,
              {0, 0, 0, 0 } ,
              {0, 0, 0, 0 } },

            { {0, c, 0, 0 }, 
              {0, c, 0, 0 } ,
              {0, c, 0, 0 } ,
              {0, c, 0, 0 } },

            { {0, 0, 0, 0 }, 
              {c, c, c, c } ,
              {0, 0, 0, 0 } ,
              {0, 0, 0, 0 } },

        };
        this.Shape_current = 0;
        this.Shape = Rotations.Extract2DArray(this.Shape_current);
    }
}

partial class Program
{
    public Mutex Mutex { get; }
    IntPtr window;
    IntPtr renderer;
    int[,] Landed;
    bool[] Input;
    public static int W_height { get; set; } = 0;
    public static int W_width { get; set; } = 0;
    public static bool Running { get; set; } = true;
    public static bool Game { get; set; } = true;
    public static bool ded { get; set; } = false; // to know if to play game over sound or not
    public int Score { get; set; } = 0;
    public int time_speed { get; set; } = 1; // used for slight difficulty scaling

    public Program()
    {
        Input = new bool[4] {false, false, false, false};
        this.Mutex = new Mutex();
        this.Landed = new int[16, 10]     //the grid of the game
        {
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
        };
    }

    private void Window_loop()
    {
        Setup(); 
        while (Running)
        {
            PollEvents();
            Render(71); 
        }
        CleanUp();
        Game = false;
    }
    private void Game_loop()
    {
        Thread.Sleep(4500);
        Random rnd = new();
        int Block_type;
        int colour = 1;
        int tmp_row, tmp_col;
        int insta_down = 1; // for speeding up bricks
        bool collision_g;
        int[,] tmp_Shape;
        Tetrominos tetromino = new Square(colour);
        while (Game)
        {
            Block_type = rnd.Next(7) + 1;
            colour = rnd.Next(6) + 1;
            switch (Block_type) {
                case 1:
                    tetromino = new Square(colour);
                    break;
                case 2:
                    tetromino = new L(colour);
                    break;
                case 3:
                    tetromino = new J(colour);
                    break;
                case 4:
                    tetromino = new Triangle(colour);
                    break;
                case 5:
                    tetromino = new Z(colour);
                    break;
                case 6:
                    tetromino = new S(colour);
                    break;
                case 7:
                    tetromino = new Line(colour);
                    break;
                default:
                    Console.WriteLine("Massive error in random block selection");
                    Game = false;
                    Running = false;
                    break;
                
            }
            collision_g = false;    //is box colliding with ground
            tmp_row = tetromino.TopLeft_row;    //for later collision checking
            tmp_col = tetromino.TopLeft_col;

            for (int j = 0; j < Landed.GetLength(1); j++) //check for game over
                if (Landed[0, j] > 0) {
                    Game = false;
                    ded = true;
                }
            if (Game == false)
                continue;

            for (int cycle = 1;; cycle++)
            {
                Draw_current(tetromino);
               
                Thread.Sleep(70);
                if (Input[0] == true && Input[2] == false) {
                    tmp_row = tetromino.TopLeft_row;
                    tmp_col = tetromino.TopLeft_col - 1;
                    Check_collision(tetromino.Shape, tmp_row, tmp_col, out bool collision_w);
                    if (collision_w == false)
                        tetromino.TopLeft_col = tmp_col;
                }
                
                else if (Input[0] == false && Input[2] == true) {
                    tmp_row = tetromino.TopLeft_row;
                    tmp_col = tetromino.TopLeft_col + 1;
                    Check_collision(tetromino.Shape, tmp_row, tmp_col, out bool collision_w);
                    if (collision_w == false)
                        tetromino.TopLeft_col = tmp_col;
                }
                if (Input[3] == true) {
                    tmp_Shape = tetromino.Rotations.Extract2DArray((tetromino.Shape_current + 1) % 4);
                    for (int i = 0; i < 3; i++) {
                        if (tetromino.TopLeft_col + i < Landed.GetLength(1)) {
                            Check_collision(tmp_Shape, tetromino.TopLeft_row, tetromino.TopLeft_col + i, out bool collision_r);
                            if (!collision_r) {
                                tetromino.Shape = tmp_Shape;
                                tetromino.Shape_current++;
                                tetromino.TopLeft_col += i;
                                break;
                            }
                        }
                        if (i == 0)
                            continue;

                        if (tetromino.TopLeft_col - i > 0) {
                            Check_collision(tmp_Shape, tetromino.TopLeft_row, tetromino.TopLeft_col - i, out bool collision_r);
                            if (!collision_r) {
                                tetromino.Shape = tmp_Shape;
                                tetromino.Shape_current++;
                                tetromino.TopLeft_col -= i;
                                break;
                            }
                        }
                    }
                    Input[3] = false;
                }
                
                if (Input[1] == true) {
                    insta_down = 5;
                    Input[1] = false;
                }
                
                if ((cycle % ((int)(5 / (time_speed * insta_down)) + 1)) == 0){
                    tmp_row = tetromino.TopLeft_row+1;
                    tmp_col = tetromino.TopLeft_col;
                    
                    Check_collision(tetromino.Shape, tmp_row, tmp_col, out collision_g);
                    Mutex.WaitOne();
                    if (!collision_g) {
                        tetromino.TopLeft_row = tmp_row;
                        tetromino.TopLeft_col = tmp_col;
                    }
                    else {
                        for (int i = 0; i < tetromino.Shape.GetLength(0); i++) {
                            for (int j = 0; j < tetromino.Shape.GetLength(1); j++) {
                                    if (tetromino.Shape[i, j] > 0)
                                Landed[tetromino.TopLeft_row + i, tetromino.TopLeft_col + j] = tetromino.Shape[i, j];
                            }
                        }  
                        Play_sound("Block_fall.wav");
                    }
                    Mutex.ReleaseMutex();
                }
                if (collision_g == true) {
                    insta_down = 1;
                    break;
                }       
            }
            Check_linefull();
        }
        Running = false;
    }
    void Play_sound(string filename)
    {
        System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.EnableRaisingEvents=false; 
            proc.StartInfo.FileName = "aplay";
            proc.StartInfo.Arguments = $"-t wav ./Audio/{filename}";
            proc.Start();
            Thread.Sleep(10);
            proc.StartInfo.FileName = "clear";
            proc.StartInfo.Arguments = $"";
            proc.Start();
    }
    
    void Check_linefull()
    {
        int line_value; // to check if the line has all spots taken
        int lines_fallen = 0; // how many line to take away
        int lowest_line = 17; // to know where to start the falling of blocks from
        Mutex.WaitOne();
        for (int i = 0; i < Landed.GetLength(0); i++) {
            line_value = 0;
            for (int j = 0; j < Landed.GetLength(1); j++) {
                if (Landed[i, j] > 0)
                    line_value++;
            }
            if (line_value == Landed.GetLength(1)) {
                for (int j = 0; j < Landed.GetLength(1); j++) {
                    Landed[i, j] = 0;
                }
                lines_fallen++;
                lowest_line = i;
            }
        }
        if (!(lines_fallen == 0)) {
            Play_sound("Line_clear.wav");
            Score = Score + (100 * lines_fallen);

            Render(1);
            Thread.Sleep(200);
            for (int k = lowest_line; k >= 0 + lines_fallen; k--) {
                for (int j = 0; j < Landed.GetLength(1); j++) {
                    Landed[k, j] = Landed[k - lines_fallen, j];
                    Landed[k - lines_fallen, j] = 0;
                }
                Render(1);
                Thread.Sleep(50);
            }
        }
        Mutex.ReleaseMutex();
    }
    void Check_collision(int[,] Shape, int tmp_row, int tmp_col,out bool collision)
    {
        Mutex.WaitOne();
        collision = false;
        for (int i = 0; i < Shape.GetLength(0); i++) {
            for (int j = 0; j < Shape.GetLength(1); j++) {
                if (Shape[i,j] > 0) {
                    if (j + tmp_col < 0 || (j + tmp_col >= Landed.GetLength(1))) {//check collision with wall
                        collision = true;
                        break;
                    }
                    if (!(tmp_row + i >= Landed.GetLength(0))) {    //check collision with ground
                        if (Landed[tmp_row + i, tmp_col + j] != 0) {    //check collision with other blocks
                            collision = true;
                        }
                    }
                    else collision = true;
                }
            }
        }       
        Mutex.ReleaseMutex();
    }
    void Draw_current(Tetrominos tetromino)
    {
        Mutex.WaitOne();
        int x,y;
        for (int i = 0; i < tetromino.Shape.GetLength(0); i++) {
            for (int j = 0; j < tetromino.Shape.GetLength(1); j++) {
                if (tetromino.Shape[i, j] > 0) {
                    x = Program.W_width / Landed.GetLength(1) * (tetromino.TopLeft_col + j);
                    y = Program.W_height / Landed.GetLength(0) * (tetromino.TopLeft_row + i);
                    Draw_box(x, y, tetromino.Shape[i, j]);
                    }
                }
            }
        Mutex.ReleaseMutex();
    }
    void Theme_loop()
    {
        Thread.Sleep(100);
        bool play = true;
        while (play) {
            Play_sound("Tetris_theme.wav");
            for (int i = 0; i < 1590; i++)
            {   
                if (Running == false && Game == false) // to make sure music does not remain after program ends
                    play = false;
                
                Thread.Sleep(100);
                if (i % 10 == 0)
                    Score++;
                
                if (i % 600 == 0)
                    time_speed++;

                if (play == false)
                    break;
            }
        
        }
        
        System.Diagnostics.Process proc = new System.Diagnostics.Process(); // killall aply so tetris theme does not play after program ends
            proc.EnableRaisingEvents=false; 
            proc.StartInfo.FileName = "killall";
            proc.StartInfo.Arguments = $"aplay";
            proc.Start();

        if (ded == true) {
            Thread.Sleep(100);
            Play_sound("Game_over.wav");
        }
    }
        
    static void Main()
    {
        var program = new Program();
        Console.WriteLine("Welcome to Tetris by Uka\n\nControlls:\nArrows left/right - movement left/right\nArrow down - Speed up the falling block\nSpace - rotate block clockwise\n//Beware of the scaling block speed//\n");
        Console.WriteLine("Input the hight of the window in pixels\n(Make sure it's slightly smaller than your monitor because the title takes up around 40 pixels)");
        string input = Console.ReadLine();
        if (!Int32.TryParse(input, out int height)) {
			Console.WriteLine("Something went wrong");
			Environment.Exit(1);
		}
        Program.W_height = height;
        Program.W_width = (int)(height*10/16);
       
        // Create the drawing thread
        var drawThread = new Thread(() => program.Window_loop());
        drawThread.Start();

        // Start the game logic in a separate thread
        var musicThread = new Thread(() => program.Theme_loop());
        musicThread.Start();

    	// Start the game logic in a separate thread
        var gameThread = new Thread(() => program.Game_loop());
        gameThread.Start();
			
        // Wait for the gameThread to complete
        gameThread.Join();
        musicThread.Join();
        Thread.Sleep(100);
        Console.WriteLine("Game Over!\nYour score:" + program.Score);
    }
    void Setup() 
    {
        // Initilizes SDL.
        if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO) < 0)
        {
            Console.WriteLine($"There was an issue initializing SDL. {SDL.SDL_GetError()}");
        }

        // Create a new window given a title, size, and passes it a flag indicating it should be shown.
        window = SDL.SDL_CreateWindow(
            "Uka Tetris SDL .NET 7",
            SDL.SDL_WINDOWPOS_UNDEFINED, 
            SDL.SDL_WINDOWPOS_UNDEFINED, 
            Program.W_width, 
            Program.W_height, 
            SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN);

        if (window == IntPtr.Zero)
        {
            Console.WriteLine($"There was an issue creating the window. {SDL.SDL_GetError()}");
        }

        // Creates a new SDL hardware renderer using the default graphics device with VSYNC enabled.
        renderer = SDL.SDL_CreateRenderer(
            window,
            -1,
            SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED |
            SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);

        if (renderer == IntPtr.Zero)
        {
            Console.WriteLine($"There was an issue creating the renderer. {SDL.SDL_GetError()}");
        }
    }
    void PollEvents()
    {
	    // Check to see if there are any events and continue to do so until the queue is empty.
	    while (SDL.SDL_PollEvent(out SDL.SDL_Event e) == 1)
	    {
  	    	switch (e.type)
  	        {
  	        	case SDL.SDL_EventType.SDL_QUIT:
  	                Running = false;
  	                break;
                
                case SDL.SDL_EventType.SDL_KEYDOWN:
                    switch (e.key.keysym.sym)
                    {
                        case SDL.SDL_Keycode.SDLK_LEFT:
                            Input[0] = true;
                            break;

                        case SDL.SDL_Keycode.SDLK_DOWN:
                            Input[1] = true;
                            break;

                        case SDL.SDL_Keycode.SDLK_RIGHT:
                            Input[2] = true;
                            break;
                        case SDL.SDL_Keycode.SDLK_SPACE:
                            Input[3] = true;
                            break;
                    }
                    break;
                case SDL.SDL_EventType.SDL_KEYUP:
                    switch (e.key.keysym.sym)
                    {
                        case SDL.SDL_Keycode.SDLK_LEFT:
                            Input[0] = false;
                            break;

                        case SDL.SDL_Keycode.SDLK_DOWN:
                            Input[1] = false;
                            break;

                        case SDL.SDL_Keycode.SDLK_RIGHT:
                            Input[2] = false;
                            break;
                        case SDL.SDL_Keycode.SDLK_SPACE:
                            Input[3] = false;
                            break;
                    }
                    break;
  	        }
  	    }
    }
    void Render(int n)
    {
        Mutex.WaitOne();
	    // Sets the color that the screen will be cleared with.
   	    SDL.SDL_SetRenderDrawColor(renderer, 55, 55, 55, 255);
	
   	    // Clears the current render surface.
        SDL.SDL_RenderClear(renderer);
        Mutex.ReleaseMutex();
        Thread.Sleep(n);

        Mutex.WaitOne();
        Render_Landed();
        
  	    // Switches out the currently presented render surface with the one we just did work on.
        SDL.SDL_RenderPresent(renderer);
        Mutex.ReleaseMutex();
    }
    void Render_Landed()
    {
        int x,y;
        for (int i = 0; i < Landed.GetLength(0); i++) {
            for (int j = 0; j < Landed.GetLength(1); j++) {
                if (Landed[i,j] > 0) {
                    x = Program.W_width / Landed.GetLength(1) * j;
                    y = Program.W_height / Landed.GetLength(0) * i;
                    Draw_box(x, y, Landed[i, j]);
                }
            }
        }
    }
    void Draw_box(int x, int y, int colour)
    {
        int space = (int)(0.1 * (Program.W_width / Landed.GetLength(0))); //space in between boxes so they are not touching
        //colour = 4; // debugging purpose
        byte R1 = 0, R2 = 0, R3 = 0, G1 = 0, G2 = 0, G3 = 0, B1 = 0, B2 = 0, B3 = 0;
        switch (colour) {   //checks which cloour the block is and picks the appropriate pallete to draw them 
            case 1: //Green block
                R1 = 200; G1 = 230; B1 = 200;
                R2 = 0;   G2 = 160; B2 = 0;
                R3 = 0;   G3 = 230; B3 = 0;
                break;
            case 2: //Red block
                R1 = 240; G1 = 190; B1 = 190;
                R2 = 160; G2 = 0;   B2 = 0; 
                R3 = 230; G3 = 0;   B3 = 0;
                break;
            case 3: //Blue block
                R1 = 200; G1 = 200; B1 = 230;
                R2 = 0;   G2 = 0;   B2 = 160;
                R3 = 0;   G3 = 0;   B3 = 230;
                break;
            case 4: // Purple block
                R1 = 220; G1 = 180; B1 = 220;
                R2 = 120; G2 = 0;   B2 = 160;
                R3 = 220; G3 = 0;   B3 = 230;
                break;
            case 5: // Orange block
                R1 = 240; G1 = 200; B1 = 160;
                R2 = 160; G2 = 80;  B2 = 0;
                R3 = 255; G3 = 140; B3 = 0;
                break;
            case 6: // Yellow block
                R1 = 255; G1 = 255; B1 = 160;
                R2 = 200; G2 = 200; B2 = 0;
                R3 = 250; G3 = 250; B3 = 0;
                break;
        }
        SDL.SDL_SetRenderDrawColor(renderer, R1, G1, B1, 255); //white shading
        var box = new SDL.SDL_Rect 
        { 
            x = x + 1,
            y = y + 1,
            w = Program.W_width / Landed.GetLength(1) - 2,
            h = Program.W_height / Landed.GetLength(0) - 2,
        };
        // Draw a triangle
        SDL.SDL_RenderFillRect(renderer, ref box);

        SDL.SDL_SetRenderDrawColor(renderer, R2, G2, B2, 255);  //triangle dark shading

        // Draw a triangle
        DrawTriangle(box.x, box.y + box.h - 1, box.w);

        SDL.SDL_SetRenderDrawColor(renderer, 0, 0, 0, 255); // black border
        box = new SDL.SDL_Rect 
        { 
            x = x + 1,
            y = y + 1,
            w = Program.W_width / Landed.GetLength(1) - 2,
            h = Program.W_height / Landed.GetLength(0) - 2,
        };
        SDL.SDL_RenderDrawRect(renderer, ref box);

        SDL.SDL_SetRenderDrawColor(renderer, R3, G3, B3, 255);   // smoll central lighter 3d cube 
        box = new SDL.SDL_Rect 
        { 
            x = x + 5 * space,
            y = y + 5 * space,
            w = Program.W_width / Landed.GetLength(1) - 10 * space,
            h = Program.W_height / Landed.GetLength(0) - 10 * space,
        };
        SDL.SDL_RenderFillRect(renderer, ref box);
    }

    void DrawTriangle(int x, int y, int size) // it's for the shading, truhly magical how something so simple makes is look so much better
    {
        // Fill the triangle (it starts from left down and goes up with each iteration)
        for (int i = y; i > y - size; i--)
        {
            SDL.SDL_RenderDrawLine(renderer, x, i, x + (y - i), y);
        }
    }
    void CleanUp()
    {
        SDL.SDL_DestroyRenderer(renderer);
        SDL.SDL_DestroyWindow(window);
        SDL.SDL_Quit();
    }
}
