using System;
using System.Numerics;
using Raylib_cs;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using static Raylib_cs.Raylib;

namespace InjectorCalculator {
	class Program {
		const int W = 800;
		const int H = 600;
		static Vector2 Origin;
		static Font TxtFnt;

		static RaylibGrid Grid;

		static void Main(string[] args) {
			SetConfigFlags(ConfigFlags.FLAG_MSAA_4X_HINT);
			SetConfigFlags(ConfigFlags.FLAG_WINDOW_HIGHDPI);
			InitWindow(W, H, "Injector Calculator");
			SetTargetFPS(30);

			Origin = new Vector2(50, 50);
			TxtFnt = LoadFont("consola.ttf");

			Grid = new RaylibGrid(new Vector2(20, 20), new Vector2(W - 100, H - 100), "PWidth [ms]", "Flow");

			LoadData();

			while (!WindowShouldClose())    // Detect window close button or ESC key
			{
				BeginDrawing();
				ClearBackground(Color.BLACK);
				Grid.Draw();
				EndDrawing();

				Vector2 MousePos = GetMousePosition();
				MousePos = new Vector2(MousePos.X, H - MousePos.Y);


				//bool Left = IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT);
				//bool Right = IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_RIGHT);
			}

			CloseWindow();
		}

		static void LoadData() {
		}
	}
}