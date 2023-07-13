// See https://aka.ms/new-console-template for more information
// Conway's, seems easy enough, but the scrolling and zooming will be the real challange

using SDL2;

partial class Program
{
    public Mutex Mutex { get; }
    IntPtr window;
    IntPtr renderer;
    int[,] Grid;
    bool[] Input;

    static int W_height  = 0;
    static int W_width  = 0;
    static int Cam_pos_x  = 0;
    static int Cam_pos_y= 0;
    static int Zoom  = 100;
    static bool Running  = true;
    public Program(int n, int m)
    {
        this.Mutex = new Mutex();
        this.Grid = new int[n, m];
        this.Input = new bool[3];
    }
    
    static void Main()
    {
        
        string input = Console.ReadLine();
        if (!Int32.TryParse(input, out int height)) {
			Console.WriteLine("Something went wrong");
			Environment.Exit(1);
		}
        Program.W_height = height;
        W_width = height;
        var program = new Program(100, 100);

        
        // Create the drawing thread
        var drawThread = new Thread(() => program.Window_loop());
        drawThread.Start();

        var conwayThread = new Thread(() => program.Conway_loop());
        conwayThread.Start();
		
        // Wait for the gameThread to complete
        conwayThread.Join();

    }
    private void Conway_loop()
    {
        while (Running)
        {
            if (Input[2] == true) {
                Next_Gen();
                Thread.Sleep(100);
            }
            
        }
    }
     void Next_Gen()
    {
        Mutex.WaitOne();
        int M = Grid.GetLength(0);
        int N = Grid.GetLength(1);
        int[,] future = new int[M,N];
 
        // Loop through every cell
        for (int l = 1; l < M - 1; l++)
        {
            for (int m = 1; m < N - 1; m++)
            {
                 
                // finding no Of Neighbours
                // that are alive
                int aliveNeighbours = 0;
                for (int i = -1; i <= 1; i++)
                    for (int j = -1; j <= 1; j++)
                        aliveNeighbours +=
                                Grid[l + i,m + j];
 
                // The cell needs to be subtracted
                // from its neighbours as it was
                // counted before
                aliveNeighbours -= Grid[l,m];
 
                // Implementing the Rules of Life
 
                // Cell is lonely and dies
                if ((Grid[l,m] == 1) && (aliveNeighbours < 2))
                    future[l,m] = 0;
 
                // Cell dies due to over population
                else if ((Grid[l,m] == 1) && (aliveNeighbours > 3))
                    future[l,m] = 0;
 
                // A new cell is born
                else if ((Grid[l,m] == 0) && (aliveNeighbours == 3))
                    future[l,m] = 1;
 
                // Remains the same
                else
                    future[l,m] = Grid[l,m];
            }
        }
        Grid = future;
        Mutex.ReleaseMutex();
    }
    private void Window_loop()
    {
        Setup(); 
        while (Running)
        {
            PollEvents();
            Render(); 
        }
        CleanUp();
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
            "Uka's game of life SDL .NET 7",
            SDL.SDL_WINDOWPOS_UNDEFINED, 
            SDL.SDL_WINDOWPOS_UNDEFINED, 
            Program.W_width, 
            Program.W_height, 
            SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN | SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE);

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
                case SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN:
                    if (e.button.button == 1) {
                        SDL.SDL_GetRelativeMouseState(out int x,out int y);
                        Input[0] = true;
                    }
                    break;

                case SDL.SDL_EventType.SDL_MOUSEBUTTONUP:
                    if (e.button.button == 1) {
                        Input[0] = false;
                    }
                    break;
                
                case SDL.SDL_EventType.SDL_MOUSEMOTION:
                    if (Input[0] == true) {
                        SDL.SDL_GetRelativeMouseState(out int x, out int y);
                        Cam_pos_x += x;
                        Cam_pos_y += y;
                    }
                    break;
                
                case SDL.SDL_EventType.SDL_KEYDOWN:
                    switch (e.key.keysym.sym)
                    {
                        case SDL.SDL_Keycode.SDLK_SPACE:
                            Mutex.WaitOne();
                            SDL.SDL_GetMouseState(out int x,out int y);

                            float tmp_x = (-Cam_pos_x + x) / (22 * (float)Zoom / 100);
                            float tmp_y = (-Cam_pos_y + y) / (22 * (float)Zoom / 100);
                                    
                            if ((int)tmp_y < Grid.GetLength(0) && tmp_y > 0)
                                if ((int)tmp_x < Grid.GetLength(1) && tmp_x > 0) {
                                    if (Grid[(int)tmp_y, (int)tmp_x] == 0)
                                        Grid[(int)tmp_y, (int)tmp_x] = 1;
                                    else if (Grid[(int)tmp_y, (int)tmp_x] == 1)
                                        Grid[(int)tmp_y, (int)tmp_x] = 0;
                                }
                            Mutex.ReleaseMutex();
                            break;

                        case SDL.SDL_Keycode.SDLK_RETURN:
                            Input[2] = Input[2] ^ true;
                            Console.WriteLine($"{Input[2]}");
                            break;
                    }
                    break;
                
                case SDL.SDL_EventType.SDL_MOUSEWHEEL:
                    Mutex.WaitOne();
                    if(e.wheel.y > 0)  // scroll up
                        Zoom = Zoom + (int)(0.02 * (float)Zoom + 1);
                    
                    
                    else if(e.wheel.y < 0)  // scroll down
                        Zoom = Zoom - (int)(0.02 * (float)Zoom + 1);
                    Mutex.ReleaseMutex();
                    break;  
            }
        }
  	    
    }
    void Render()
    {
        Mutex.WaitOne();
	    // Sets the color that the screen will be cleared with.
   	    
        SDL.SDL_SetRenderDrawColor(renderer, 0, 0, 0, 200);

   	    // Clears the current render surface.
        SDL.SDL_RenderClear(renderer);
        Mutex.ReleaseMutex();
        Thread.Sleep(10);
        Mutex.WaitOne();
        Draw_Grid();
        
  	    // Switches out the currently presented render surface with the one we just did work on.
        SDL.SDL_RenderPresent(renderer);
        Mutex.ReleaseMutex();
        
    }
    void Draw_Grid()
    {
        int x,y;
        for (int i = 0; i < Grid.GetLength(0); i++) {
            for (int j = 0; j < Grid.GetLength(1); j++) {
                x = 22 * Zoom  * j;
                y = 22 * Zoom  * i;
                Draw_box(x, y, Grid[i,j]);
            }
        }
    }
    void Draw_box(int x, int y, int type)
    {
        byte R = 0, G = 0, B = 0;
        switch (type) {
            case 0:
                R = 20; G = 20; B = 20;
                break;
            case 1:
                R = 20; G = 230; B = 20;
                break;
            default:
                Console.WriteLine("Bad?");
                break;
        }
        SDL.SDL_SetRenderDrawColor(renderer, R, G, B, 255);
        
        int border = (1 * Zoom / 100);
        

        var box = new SDL.SDL_Rect 
        { 
            x = x / 100 + Cam_pos_x + border,
            y = y / 100 + Cam_pos_y + border,
            w = 20 * Zoom / 100,
            h = 20 * Zoom / 100,
        };
        SDL.SDL_RenderFillRect(renderer, ref box);
    }
    void CleanUp()
    {
        SDL.SDL_DestroyRenderer(renderer);
        SDL.SDL_DestroyWindow(window);
        SDL.SDL_Quit();
    }
}
