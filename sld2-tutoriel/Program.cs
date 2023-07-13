using SDL2;
using static SDL2.SDL;

IntPtr window;
IntPtr renderer;
bool running = true;

void Setup() 
{
    // Initilizes SDL.
    if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO) < 0)
    {
        Console.WriteLine($"There was an issue initializing SDL. {SDL.SDL_GetError()}");
    }

    // Create a new window given a title, size, and passes it a flag indicating it should be shown.
    window = SDL.SDL_CreateWindow(
        "SDL .NET 6 Tutorial",
        SDL.SDL_WINDOWPOS_UNDEFINED, 
        SDL.SDL_WINDOWPOS_UNDEFINED, 
        640, 
        480, 
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

void Render()
{
	// Sets the color that the screen will be cleared with.
   	SDL.SDL_SetRenderDrawColor(renderer, 70, 70, 70, 255);
	
   	// Clears the current render surface.
    SDL.SDL_RenderClear(renderer);
	
    // Set the color to red before drawing our shape
    //SDL.SDL_SetRenderDrawColor(renderer, 255, 0, 0, 255);

    // Draw a line from top left to bottom right
    //SDL.SDL_RenderDrawLine(renderer, 0, 0, 640, 480);
    
    // Set the color to red before drawing our shape
    //SDL.SDL_SetRenderDrawColor(renderer, 0, 255, 0, 255);
    
	// Draws a point at (20, 20) using the currently set color.
	//SDL.SDL_RenderDrawPoint(renderer, 20, 20);
	
	// Set the color to red before drawing our shape
    //SDL.SDL_SetRenderDrawColor(renderer, 0, 0, 255, 255);
	
	// Specify the coordinates for our rectangle we will be drawing.
	//var floor = new SDL.SDL_Rect 
	//{ 
    //	x = 0,
   	//	y = 450,
    //	w = 640,
    //	h = 30
	//};

	// Draw a filled in rectangle.
	//SDL.SDL_RenderFillRect(renderer, ref floor);
	
  	// Switches out the currently presented render surface with the one we just did work on.
    SDL.SDL_RenderPresent(renderer);
}

void CleanUp()
{
    SDL.SDL_DestroyRenderer(renderer);
    SDL.SDL_DestroyWindow(window);
    SDL.SDL_Quit();
}

Setup();
while (running)
{	
	PollEvents();
	Render();
}
CleanUp();
