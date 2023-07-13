// See https://aka.ms/new-console-template for more information
// 74 with sdl2 omg holy shit how am i gonna make this, wtf.

using System;
using SDL2;


class Tower
{
    private List<Disk> disks;

    public Tower()
    {
        disks = new List<Disk>();
    }

    public bool IsEmpty()
    {
        return disks.Count == 0;
    }

    public void AddDisk(Disk disk)
    {
        disks.Add(disk);
    }

    public Disk RemoveTopDisk()
    {
        if (!IsEmpty())
        {
            Disk topDisk = disks[disks.Count - 1];
            disks.RemoveAt(disks.Count - 1);
            return topDisk;
        }
        return null;
    }

    public void DrawTower(int x, IntPtr renderer)
    {
        SDL.SDL_SetRenderDrawColor(renderer, 0, 0, 0, 255);
        var pole = new SDL.SDL_Rect 
	    { 
        	x = x-10,
   	    	y = 200,
        	w = 20,
        	h = 900
	    };
        SDL.SDL_RenderFillRect(renderer, ref pole);

        int counter = 1;
        foreach (Disk disk in disks)
        {
            disk.DrawDisk(x, 960 - (50 + counter * 30), renderer);
            counter++;
        }
    }
}
class Disk
{
    public int Size { get; private set; }

    public Disk(int size)
    {
        Size = size;
    }

    public void DrawDisk(int x, int y, IntPtr renderer)
    {
        // Set the color to green before drawing our shape
        byte gradient = Convert.ToByte(Size*15);
        
        SDL.SDL_SetRenderDrawColor(renderer, gradient, 255, gradient, 255);

        var disk = new SDL.SDL_Rect 
	    { 
        	x = x-(20 + Size*20)/2,
   	    	y = y,
        	w = 20 + Size*20,
        	h = 30
	    };

	    // Draw a filled in rectangle.
	    SDL.SDL_RenderFillRect(renderer, ref disk);

    }
}

class HanoiTowerSolver
{
    public Tower TowerA { get; }
    public Tower TowerB { get; }
    public Tower TowerC { get; }

    public HanoiTowerSolver(int n)
    {
        TowerA = new Tower();
        TowerB = new Tower();
        TowerC = new Tower();

        for (int i = n; i > 0; i--)
        {
            Disk tmp = new(i);
            TowerA.AddDisk(tmp);
      //      Program.update = true;
        }
    }


    public void Solve(int numDisks, Program program)
    {
        // Move the disks from tower A to tower C using tower B as the auxiliary tower
        MoveDisks(numDisks, TowerA, TowerC, TowerB);
        Console.Beep();
        Thread.Sleep(1300);
        Program.running = false;
    }

    private void MoveDisks(int numDisks, Tower source, Tower target, Tower auxiliary)
    {
        if (numDisks > 0)
        {
            MoveDisks(numDisks - 1, source, auxiliary, target);
            Console.Beep();
            Thread.Sleep(600);
            var disk = source.RemoveTopDisk();
            target.AddDisk(disk);

            MoveDisks(numDisks - 1, auxiliary, target, source);
        }
    }
}
partial class Program
{
    IntPtr window;
    IntPtr renderer;
    public static bool running { get; set; } = true;
    //public static bool update { get; set; } = true; // a bool for threads to communitate when to draw the scene again
    
     // Define the drawing thread method
      private void DrawThread(HanoiTowerSolver solver)
    {
        // SDL2 drawing logic
        // Continuously update the visual representation of the towers and disks
        Setup();
        while (running)
        {
            PollEvents();
          //  if (Program.update) {
                Render(solver);
            //    Program.update = false;
            }
       // }
    }
    
    static void Main()
    {
        Console.WriteLine("How many pegs do you have? (current version supports 1-17, but they get too slow)");
		string input = Console.ReadLine();
		
		if (!Int32.TryParse(input, out int n)) {
			Console.WriteLine("Error in readin input");
			Environment.Exit(1);
		}
        if (n > 17 || n < 0) {
            Console.WriteLine("Unsuported number of pegs");
			Environment.Exit(1);
       }
        // Create an instance of the HanoiTowerSolver
        var solver = new HanoiTowerSolver(n);
        
       
        // Create the drawing thread
        var program = new Program();
        var drawThread = new Thread(() => program.DrawThread(solver));
        drawThread.Start();

    	// Solve the Hanoi Tower problem in a separate thread
        var solveThread = new Thread(() => solver.Solve(n, program));
        solveThread.Start();
			
        // Wait for the solveThread to complete
        solveThread.Join();

        program.CleanUp();
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
            "SDL .NET 6 Hanoi tower solver",
            SDL.SDL_WINDOWPOS_UNDEFINED, 
            SDL.SDL_WINDOWPOS_UNDEFINED, 
            1280, 
            960, 
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
  	            running = false;
  	            break;
  	        }
  	    }
    }
    void Render(HanoiTowerSolver solver)
    {
	    // Sets the color that the screen will be cleared with.
   	    SDL.SDL_SetRenderDrawColor(renderer, 70, 70, 70, 255);
	
   	    // Clears the current render surface.
        SDL.SDL_RenderClear(renderer);

        SDL.SDL_SetRenderDrawColor(renderer, 0, 0, 0, 255);
	
        // Specify the coordinates for our floor.
	    var floor = new SDL.SDL_Rect 
	    { 
        	x = 0,
   	    	y = 910,
        	w = 1280,
        	h = 50
	    };

	    // Draw a filled in rectangle (floor).
	    SDL.SDL_RenderFillRect(renderer, ref floor);

        
        solver.TowerA.DrawTower(320 * 1, renderer); // Call the DrawTower method on the instance
        solver.TowerB.DrawTower(320 * 2, renderer);
        solver.TowerC.DrawTower(320 * 3, renderer);

  	    // Switches out the currently presented render surface with the one we just did work on.
        SDL.SDL_RenderPresent(renderer);
    }
    void CleanUp()
    {
        SDL.SDL_DestroyRenderer(renderer);
        SDL.SDL_DestroyWindow(window);
        SDL.SDL_Quit();
    }
}
