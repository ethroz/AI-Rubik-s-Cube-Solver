using System;
using UnityEngine;

public class CubeSolver
{
    private string[,,] CurrentColors = new string[3, 3, 6];
    private int CurrentMoveNum = -1;
    private float[] CurrentMoves = new float[] { 0 };
    private bool BottomCross = false;
    private bool BottomCorners = false;
    private bool MiddleEdges = false;
    private bool TopCross = false;
    private bool TopCorners = false;
    private bool TopCornersPermutation = false;
    private bool FewTouches = false;
    private bool TopEdgesPermutation = false;
    public bool Solved = false;

    public float CurrentMovesMethod(string[,,] Scramble)
    {
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    CurrentColors[k, j, i] = Scramble[k, j, i];
                }
            }
        }

        CurrentMoveNum++;

        if (CurrentMoveNum < CurrentMoves.Length)
            return CurrentMoves[CurrentMoveNum];
        else if (!BottomCross)
            CurrentMoves = BottomCrossMethod();
        else if (!BottomCorners)
            CurrentMoves = BottomCornersMethod();
        else if (!MiddleEdges)
            CurrentMoves = MiddleEdgesMethod();
        else if (!TopCross)
            CurrentMoves = TopCrossMethod();
        else if (!TopCorners)
            CurrentMoves = TopCornersMethod();
        else if (!TopCornersPermutation)
            CurrentMoves = TopCornersPermutationMethod();
        else if (!FewTouches)
            CurrentMoves = FewTouchesMethod();
        else if (!TopEdgesPermutation)
            CurrentMoves = TopEdgesPermutationMethod();
        else
            return 7;

        CurrentMoveNum = 0;
        return CurrentMoves[CurrentMoveNum];
    }

    public float[] FindSolution(string[,,] Scramble)
    {
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    CurrentColors[k, j, i] = Scramble[k, j, i];
                }
            }
        }
        float[] solution = new float[0];

        while (!BottomCross)
        {
            float[] temp2 = BottomCrossMethod();
            if (temp2[0] == 0)
                break;
            float[] temp1 = solution;
            solution = new float[temp1.Length + temp2.Length];

            for (int i = 0; i < temp1.Length; i++)
            {
                solution[i] = temp1[i];
            }
            for (int i = 0; i < temp2.Length; i++)
            {
                solution[temp1.Length + i] = temp2[i];
                RotateSides(temp2[i]);
            }
        }
        while (!BottomCorners)
        {
            float[] temp2 = BottomCornersMethod();
            if (temp2[0] == 0)
                break;
            float[] temp1 = solution;

            solution = new float[temp1.Length + temp2.Length];
            for (int i = 0; i < temp1.Length; i++)
            {
                solution[i] = temp1[i];
            }
            for (int i = 0; i < temp2.Length; i++)
            {
                solution[temp1.Length + i] = temp2[i];
                RotateSides(temp2[i]);
            }
        }
        while (!MiddleEdges)
        {
            float[] temp2 = MiddleEdgesMethod();
            if (temp2[0] == 0)
                break;
            float[] temp1 = solution;

            solution = new float[temp1.Length + temp2.Length];
            for (int i = 0; i < temp1.Length; i++)
            {
                solution[i] = temp1[i];
            }
            for (int i = 0; i < temp2.Length; i++)
            {
                solution[temp1.Length + i] = temp2[i];
                RotateSides(temp2[i]);
            }
        }
        while (!TopCross)
        {
            float[] temp2 = TopCrossMethod();
            if (temp2[0] == 0)
                break;
            float[] temp1 = solution;

            solution = new float[temp1.Length + temp2.Length];
            for (int i = 0; i < temp1.Length; i++)
            {
                solution[i] = temp1[i];
            }
            for (int i = 0; i < temp2.Length; i++)
            {
                solution[temp1.Length + i] = temp2[i];
                RotateSides(temp2[i]);
            }
        }
        while (!TopCorners)
        {
            float[] temp2 = TopCornersMethod();
            if (temp2[0] == 0)
                break;
            float[] temp1 = solution;

            solution = new float[temp1.Length + temp2.Length];
            for (int i = 0; i < temp1.Length; i++)
            {
                solution[i] = temp1[i];
            }
            for (int i = 0; i < temp2.Length; i++)
            {
                solution[temp1.Length + i] = temp2[i];
                RotateSides(temp2[i]);
            }
        }
        while (!TopCornersPermutation)
        {
            float[] temp2 = TopCornersPermutationMethod();
            if (temp2[0] == 0)
                break;
            float[] temp1 = solution;

            solution = new float[temp1.Length + temp2.Length];
            for (int i = 0; i < temp1.Length; i++)
            {
                solution[i] = temp1[i];
            }
            for (int i = 0; i < temp2.Length; i++)
            {
                solution[temp1.Length + i] = temp2[i];
                RotateSides(temp2[i]);
            }
        }
        while (!FewTouches)
        {
            float[] temp2 = FewTouchesMethod();
            if (temp2[0] == 0)
                break;
            float[] temp1 = solution;

            solution = new float[temp1.Length + temp2.Length];
            for (int i = 0; i < temp1.Length; i++)
            {
                solution[i] = temp1[i];
            }
            for (int i = 0; i < temp2.Length; i++)
            {
                solution[temp1.Length + i] = temp2[i];
                RotateSides(temp2[i]);
            }
        }
        while (!TopEdgesPermutation)
        {
            float[] temp2 = TopEdgesPermutationMethod();
            if (temp2[0] == 0)
                break;
            float[] temp1 = solution;

            solution = new float[temp1.Length + temp2.Length];
            for (int i = 0; i < temp1.Length; i++)
            {
                solution[i] = temp1[i];
            }
            for (int i = 0; i < temp2.Length; i++)
            {
                solution[temp1.Length + i] = temp2[i];
                RotateSides(temp2[i]);
            }
        }

        return solution;
    }

    private void RotateSides(float side)
    {
        float direction = side / Mathf.Abs(side);
        switch ((int)Mathf.Abs(side))
        {
            case 1:
                string[,] face1 = new string[3, 3];
                string[] faceEdge1 = new string[12];

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        face1[j, i] = CurrentColors[j, i, 0];
                    }
                }
                for (int i = 0; i < 12; i++)
                {
                    faceEdge1[i] = CurrentColors[(int)(-Mathf.Abs(i - (float)8.5) + 8.5) % 3, 2, Mathf.FloorToInt(i / 3) + 1];
                }

                if (direction == 1)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            CurrentColors[i, Mathf.Abs(j - 2), 0] = face1[j, i];
                        }
                    }
                    for (int i = 0; i < 12; i++)
                    {
                        CurrentColors[(int)(-Mathf.Abs(i - (float)8.5) + 8.5) % 3, 2, Mathf.FloorToInt(i / 3) + 1] = faceEdge1[(i + 3) % 12];
                    }
                }
                else if (direction == -1)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            CurrentColors[Mathf.Abs(i - 2), j, 0] = face1[j, i];
                        }
                    }
                    for (int i = 11; i > -1; i--)
                    {
                        CurrentColors[(int)(-Mathf.Abs(i - (float)8.5) + 8.5) % 3, 2, Mathf.FloorToInt(i / 3) + 1] = faceEdge1[(i + 9) % 12];
                    }
                }
                break;
            case 2:
                string[,] face2 = new string[3, 3];
                string[] faceEdge2 = new string[12];

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        face2[j, i] = CurrentColors[j, i, 1];
                    }
                }
                for (int i = 0; i < 12; i++)
                {
                    faceEdge2[i] = CurrentColors[0, (int)(Mathf.Abs(i - (float)5.5) + 5.5) % 3, Mathf.CeilToInt(3 * Mathf.Sin((Mathf.Floor((-i - 3) / 3) % 5) + 5) + (float)1.4)];
                }

                if (direction == 1)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            CurrentColors[i, Mathf.Abs(j - 2), 1] = face2[j, i];
                        }
                    }
                    for (int i = 11; i > -1; i--)
                    {
                        CurrentColors[0, (int)(Mathf.Abs(i - (float)5.5) + 5.5) % 3, Mathf.CeilToInt(3 * Mathf.Sin((Mathf.Floor((-i - 3) / 3) % 5) + 5) + (float)1.4)] = faceEdge2[(i + 9) % 12];
                    }
                }
                else if (direction == -1)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            CurrentColors[Mathf.Abs(i - 2), j, 1] = face2[j, i];
                        }
                    }
                    for (int i = 0; i < 12; i++)
                    {
                        CurrentColors[0, (int)(Mathf.Abs(i - (float)5.5) + 5.5) % 3, Mathf.CeilToInt(3 * Mathf.Sin((Mathf.Floor((-i - 3) / 3) % 5) + 5) + (float)1.4)] = faceEdge2[(i + 3) % 12];
                    }
                }
                break;
            case 3:
                string[,] face3 = new string[3, 3];
                string[] faceEdge3 = new string[12];

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        face3[j, i] = CurrentColors[j, i, 2];
                    }
                }
                for (int i = 0; i < 12; i++)
                {
                    faceEdge3[i] = CurrentColors[Math.Sign((i + 1) * (i - 5.5) * (i - 11.5)) * Mathf.RoundToInt((float)Math.Tanh(((i + 3) % 6) - 1) + Math.Sign((i + 1) * (i - 5.5) * (i - 11.5))), (int)((((double)Math.Sign((i + 0.5) * (i - 2.5) * (i - 5.5) * (i - 8.5) * (i - 11.5)) / (double)2) + 0.5) * (double)(-Mathf.Abs(i - 4) + 4)), Mathf.CeilToInt((float)3.1 * Mathf.Sin(Mathf.Floor((i / 3) + 16) % 12) + (float)2.9)];
                }

                if (direction == 1)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            CurrentColors[i, Mathf.Abs(j - 2), 2] = face3[j, i];
                        }
                    }
                    for (int i = 11; i > -1; i--)
                    {
                        CurrentColors[Math.Sign((i + 1) * (i - 5.5) * (i - 11.5)) * Mathf.RoundToInt((float)Math.Tanh(((i + 3) % 6) - 1) + Math.Sign((i + 1) * (i - 5.5) * (i - 11.5))), (int)((((double)Math.Sign((i + 0.5) * (i - 2.5) * (i - 5.5) * (i - 8.5) * (i - 11.5)) / (double)2) + 0.5) * (double)(-Mathf.Abs(i - 4) + 4)), Mathf.CeilToInt((float)3.1 * Mathf.Sin(Mathf.Floor((i / 3) + 16) % 12) + (float)2.9)] = faceEdge3[(i + 9) % 12];
                    }
                }
                else if (direction == -1)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            CurrentColors[Mathf.Abs(i - 2), j, 2] = face3[j, i];
                        }
                    }
                    for (int i = 0; i < 12; i++)
                    {
                        CurrentColors[Math.Sign((i + 1) * (i - 5.5) * (i - 11.5)) * Mathf.RoundToInt((float)Math.Tanh(((i + 3) % 6) - 1) + Math.Sign((i + 1) * (i - 5.5) * (i - 11.5))), (int)((((double)Math.Sign((i + 0.5) * (i - 2.5) * (i - 5.5) * (i - 8.5) * (i - 11.5)) / (double)2) + 0.5) * (double)(-Mathf.Abs(i - 4) + 4)), Mathf.CeilToInt((float)3.1 * Mathf.Sin(Mathf.Floor((i / 3) + 16) % 12) + (float)2.9)] = faceEdge3[(i + 3) % 12];
                    }
                }
                break;
            case 4:
                string[,] face4 = new string[3, 3];
                string[] faceEdge4 = new string[12];

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        face4[j, i] = CurrentColors[j, i, 3];
                    }
                }
                for (int i = 0; i < 12; i++)
                {
                    faceEdge4[i] = CurrentColors[2, (int)(Mathf.Abs(i - (float)5.5) + 5.5) % 3, Mathf.CeilToInt(3 * Mathf.Sin((Mathf.Floor((-i - 3) / 3) % 5) + 5) + (float)1.4)];
                }

                if (direction == 1)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            CurrentColors[i, Mathf.Abs(j - 2), 3] = face4[j, i];
                        }
                    }
                    for (int i = 0; i < 12; i++)
                    {
                        CurrentColors[2, (int)(Mathf.Abs(i - (float)5.5) + 5.5) % 3, Mathf.CeilToInt(3 * Mathf.Sin((Mathf.Floor((-i - 3) / 3) % 5) + 5) + (float)1.4)] = faceEdge4[(i + 3) % 12];
                    }
                }
                else if (direction == -1)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            CurrentColors[Mathf.Abs(i - 2), j, 3] = face4[j, i];
                        }
                    }
                    for (int i = 11; i > -1; i--)
                    {
                        CurrentColors[2, (int)(Mathf.Abs(i - (float)5.5) + 5.5) % 3, Mathf.CeilToInt(3 * Mathf.Sin((Mathf.Floor((-i - 3) / 3) % 5) + 5) + (float)1.4)] = faceEdge4[(i + 9) % 12];
                    }
                }
                break;
            case 5:
                string[,] face5 = new string[3, 3];
                string[] faceEdge5 = new string[12];

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        face5[j, i] = CurrentColors[j, i, 4];
                    }
                }
                for (int i = 0; i < 12; i++)
                {
                    faceEdge5[i] = CurrentColors[Mathf.RoundToInt((float)2.49 * Mathf.Pow((float)Math.E, (-1) * ((float)(i - 7)) / (float)3 * ((float)(i - 7)) / (float)3)), (int)((((double)Math.Sign((i + 0.5) * (i - 2.5) * (i - 5.5) * (i - 8.5) * (i - 11.5)) / (double)2) + 0.5) * (double)-Mathf.Abs(i - 4) + Math.Sign((i + 0.5) * (i - 2.5) * (i - 5.5) * (i - 8.5) * (i - 11.5)) + 3), Mathf.CeilToInt((float)3.1 * Mathf.Sin(Mathf.Floor((i / 3) + 16) % 12) + (float)2.9)];
                }

                if (direction == 1)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            CurrentColors[i, Mathf.Abs(j - 2), 4] = face5[j, i];
                        }
                    }
                    for (int i = 11; i > -1; i--)
                    {
                        CurrentColors[Mathf.RoundToInt((float)2.49 * Mathf.Pow((float)Math.E, (-1) * ((float)(i - 7)) / (float)3 * ((float)(i - 7)) / (float)3)), (int)((((double)Math.Sign((i + 0.5) * (i - 2.5) * (i - 5.5) * (i - 8.5) * (i - 11.5)) / (double)2) + 0.5) * (double)-Mathf.Abs(i - 4) + Math.Sign((i + 0.5) * (i - 2.5) * (i - 5.5) * (i - 8.5) * (i - 11.5)) + 3), Mathf.CeilToInt((float)3.1 * Mathf.Sin(Mathf.Floor((i / 3) + 16) % 12) + (float)2.9)] = faceEdge5[(i + 9) % 12];
                    }
                }
                else if (direction == -1)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            CurrentColors[Mathf.Abs(i - 2), j, 4] = face5[j, i];
                        }
                    }
                    for (int i = 0; i < 12; i++)
                    {
                        CurrentColors[Mathf.RoundToInt((float)2.49 * Mathf.Pow((float)Math.E, (-1) * ((float)(i - 7)) / (float)3 * ((float)(i - 7)) / (float)3)), (int)((((double)Math.Sign((i + 0.5) * (i - 2.5) * (i - 5.5) * (i - 8.5) * (i - 11.5)) / (double)2) + 0.5) * (double)-Mathf.Abs(i - 4) + Math.Sign((i + 0.5) * (i - 2.5) * (i - 5.5) * (i - 8.5) * (i - 11.5)) + 3), Mathf.CeilToInt((float)3.1 * Mathf.Sin(Mathf.Floor((i / 3) + 16) % 12) + (float)2.9)] = faceEdge5[(i + 3) % 12];
                    }
                }
                break;
            case 6:
                string[,] face6 = new string[3, 3];
                string[] faceEdge6 = new string[12];

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        face6[j, i] = CurrentColors[j, i, 5];
                    }
                }
                for (int i = 0; i < 12; i++)
                {
                    faceEdge6[i] = CurrentColors[(int)(-Mathf.Abs(i - (float)8.5) + 8.5) % 3, 0, Mathf.FloorToInt(i / 3) + 1];
                }

                if (direction == 1)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            CurrentColors[i, Mathf.Abs(j - 2), 5] = face6[j, i];
                        }
                    }
                    for (int i = 0; i < 12; i++)
                    {
                        CurrentColors[(int)(-Mathf.Abs(i - (float)8.5) + 8.5) % 3, 0, Mathf.FloorToInt(i / 3) + 1] = faceEdge6[(i + 3) % 12];
                    }
                }
                else if (direction == -1)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            CurrentColors[Mathf.Abs(i - 2), j, 5] = face6[j, i];
                        }
                    }
                    for (int i = 11; i > -1; i--)
                    {
                        CurrentColors[(int)(-Mathf.Abs(i - (float)8.5) + 8.5) % 3, 0, Mathf.FloorToInt(i / 3) + 1] = faceEdge6[(i + 9) % 12];

                    }
                }
                break;
        }
    }

    private string FindAdjacentEdge(int posx, int posy, int side)
    {
        string adColor = "";

        switch (side)
        {
            case 0:
                switch (posx)
                {
                    case 0:
                        adColor = CurrentColors[1, 2, 1];
                        break;
                    case 1:
                        switch (posy)
                        {
                            case 0:
                                adColor = CurrentColors[1, 2, 2];
                                break;
                            case 2:
                                adColor = CurrentColors[1, 2, 4];
                                break;
                        }
                        break;
                    case 2:
                        adColor = CurrentColors[1, 2, 3];
                        break;
                }
                break;
            case 1:
                switch (posx)
                {
                    case 0:
                        adColor = CurrentColors[0, 1, 4];
                        break;
                    case 1:
                        switch (posy)
                        {
                            case 0:
                                adColor = CurrentColors[0, 1, 5];
                                break;
                            case 2:
                                adColor = CurrentColors[0, 1, 0];
                                break;
                        }
                        break;
                    case 2:
                        adColor = CurrentColors[0, 1, 2];
                        break;
                }
                break;
            case 2:
                switch (posx)
                {
                    case 0:
                        adColor = CurrentColors[2, 1, 1];
                        break;
                    case 1:
                        switch (posy)
                        {
                            case 0:
                                adColor = CurrentColors[1, 0, 5];
                                break;
                            case 2:
                                adColor = CurrentColors[1, 0, 0];
                                break;
                        }
                        break;
                    case 2:
                        adColor = CurrentColors[0, 1, 3];
                        break;
                }
                break;
            case 3:
                switch (posx)
                {
                    case 0:
                        adColor = CurrentColors[2, 1, 2];
                        break;
                    case 1:
                        switch (posy)
                        {
                            case 0:
                                adColor = CurrentColors[2, 1, 5];
                                break;
                            case 2:
                                adColor = CurrentColors[2, 1, 0];
                                break;
                        }
                        break;
                    case 2:
                        adColor = CurrentColors[2, 1, 4];
                        break;
                }
                break;
            case 4:
                switch (posx)
                {
                    case 0:
                        adColor = CurrentColors[0, 1, 1];
                        break;
                    case 1:
                        switch (posy)
                        {
                            case 0:
                                adColor = CurrentColors[1, 2, 5];
                                break;
                            case 2:
                                adColor = CurrentColors[1, 2, 0];
                                break;
                        }
                        break;
                    case 2:
                        adColor = CurrentColors[2, 1, 3];
                        break;
                }
                break;
        }

        return adColor;
    }

    private string FindAdjacentCorners(int posx, int posy, int side)
    {
        string matchColors = "";

        switch (side)
        {
            case 0:
                switch (posx)
                {
                    case 0:
                        switch (posy)
                        {
                            case 0:
                                matchColors = CurrentColors[0, 2, 2] + CurrentColors[2, 2, 1];
                                break;
                            case 2:
                                matchColors = CurrentColors[0, 2, 1] + CurrentColors[0, 2, 4];
                                break;
                        }
                        break;
                    case 2:
                        switch (posy)
                        {
                            case 0:
                                matchColors = CurrentColors[0, 2, 3] + CurrentColors[2, 2, 2];
                                break;
                            case 2:
                                matchColors = CurrentColors[2, 2, 4] + CurrentColors[2, 2, 3];
                                break;
                        }
                        break;
                }
                break;
            case 1:
                switch (posx)
                {
                    case 0:
                        switch (posy)
                        {
                            case 0:
                                matchColors = CurrentColors[0, 2, 5] + CurrentColors[0, 0, 4];
                                break;
                            case 2:
                                matchColors = CurrentColors[0, 2, 4] + CurrentColors[0, 2, 0];
                                break;
                        }
                        break;
                    case 2:
                        switch (posy)
                        {
                            case 0:
                                matchColors = CurrentColors[0, 0, 2] + CurrentColors[0, 0, 5];
                                break;
                            case 2:
                                matchColors = CurrentColors[0, 0, 0] + CurrentColors[0, 2, 2];
                                break;
                        }
                        break;
                }
                break;
            case 2:
                switch (posx)
                {
                    case 0:
                        switch (posy)
                        {
                            case 0:
                                matchColors = CurrentColors[0, 0, 5] + CurrentColors[2, 0, 1];
                                break;
                            case 2:
                                matchColors = CurrentColors[2, 2, 1] + CurrentColors[0, 0, 0];
                                break;
                        }
                        break;
                    case 2:
                        switch (posy)
                        {
                            case 0:
                                matchColors = CurrentColors[0, 0, 3] + CurrentColors[2, 0, 5];
                                break;
                            case 2:
                                matchColors = CurrentColors[2, 0, 0] + CurrentColors[0, 2, 3];
                                break;
                        }
                        break;
                }
                break;
            case 3:
                switch (posx)
                {
                    case 0:
                        switch (posy)
                        {
                            case 0:
                                matchColors = CurrentColors[2, 0, 5] + CurrentColors[2, 0, 2];
                                break;
                            case 2:
                                matchColors = CurrentColors[2, 2, 2] + CurrentColors[2, 0, 0];
                                break;
                        }
                        break;
                    case 2:
                        switch (posy)
                        {
                            case 0:
                                matchColors = CurrentColors[2, 0, 4] + CurrentColors[2, 2, 5];
                                break;
                            case 2:
                                matchColors = CurrentColors[2, 2, 0] + CurrentColors[2, 2, 4];
                                break;
                        }
                        break;
                }
                break;
            case 4:
                switch (posx)
                {
                    case 0:
                        switch (posy)
                        {
                            case 0:
                                matchColors = CurrentColors[0, 0, 1] + CurrentColors[0, 2, 0];
                                break;
                            case 2:
                                matchColors = CurrentColors[0, 2, 0] + CurrentColors[0, 2, 1];
                                break;
                        }
                        break;
                    case 2:
                        switch (posy)
                        {
                            case 0:
                                matchColors = CurrentColors[2, 2, 5] + CurrentColors[2, 0, 3];
                                break;
                            case 2:
                                matchColors = CurrentColors[2, 2, 3] + CurrentColors[2, 2, 0];
                                break;
                        }
                        break;
                }
                break;
        }

        return matchColors;
    }

    private float[] BottomCrossMethod()
    {
        float[] temp = new float[] { 0 };
        int posx = 0;
        int posy = 0;
        int side = 0;
        string adColor;

        for (int i = 0; i < 5; i++)
        {
            for (int k = 1; k < 9; k += 2)
            {
                if (CurrentColors[k % 3, Mathf.FloorToInt(k / 3), i] == "W")
                {
                    posx = k % 3;
                    posy = Mathf.FloorToInt(k / 3);
                    side = i;
                    goto SkipFind;
                }
            }
        }
        if (CurrentColors[1, 0, 1] != "B")
            temp = new float[] { 2, 2 };
        else if (CurrentColors[1, 0, 2] != "R")
            temp = new float[] { 3, 3 };
        else if (CurrentColors[1, 0, 4] != "O")
            temp = new float[] { 5, 5 };
        else if (CurrentColors[1, 0, 3] != "G")
            temp = new float[] { 4, 4 };
        else
            BottomCross = true;

        goto Finish;
        SkipFind:
        adColor = FindAdjacentEdge(posx, posy, side);

        switch (adColor)
        {
            case "B":
                switch (side)
                {
                    case 0:
                        switch (posx)
                        {
                            case 0:
                                temp = new float[] { 2, 2 };
                                break;
                            case 1:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { 1, 2, 2 };
                                        break;
                                    case 2:
                                        temp = new float[] { -1, 2, 2 };
                                        break;
                                }
                                break;
                            case 2:
                                temp = new float[] { 1, 1, 2, 2 };
                                break;
                        }
                        break;
                    case 1:
                        switch (posx)
                        {
                            case 0:
                                temp = new float[] { 6, -5, -6 };
                                break;
                            case 1:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { 2, 6, -5, -6 };
                                        break;
                                    case 2:
                                        temp = new float[] { 2, -6, -3, 6 };
                                        break;
                                }
                                break;
                            case 2:
                                temp = new float[] { -6, -3, 6 };
                                break;
                        }
                        break;
                    case 2:
                        switch (posx)
                        {
                            case 0:
                                temp = new float[] { 2 };
                                break;
                            case 1:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { 3, 2 };
                                        break;
                                    case 2:
                                        temp = new float[] { -3, 2, 3 };
                                        break;
                                }
                                break;
                            case 2:
                                temp = new float[] { 3, 3, 2, 3, 3 };
                                break;
                        }
                        break;
                    case 3:
                        switch (posx)
                        {
                            case 0:
                                temp = new float[] { -6, 3, 6 };
                                break;
                            case 1:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { 4, -6, 3, 6 };
                                        break;
                                    case 2:
                                        temp = new float[] { 4, 6, 5, -6 };
                                        break;
                                }
                                break;
                            case 2:
                                temp = new float[] { 6, 5, -6 };
                                break;
                        }
                        break;
                    case 4:
                        switch (posx)
                        {
                            case 0:
                                temp = new float[] { -2 };
                                break;
                            case 1:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { 5, -2 };
                                        break;
                                    case 2:
                                        temp = new float[] { -5, -2, 5 };
                                        break;
                                }
                                break;
                            case 2:
                                temp = new float[] { 5, 5, -2, 5, 5 };
                                break;
                        }
                        break;
                }
                break;
            case "R":
                switch (side)
                {
                    case 0:
                        switch (posx)
                        {
                            case 0:
                                temp = new float[] { -1, 3, 3 };
                                break;
                            case 1:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { 3, 3 };
                                        break;
                                    case 2:
                                        temp = new float[] { 1, 1, 3, 3 };
                                        break;
                                }
                                break;
                            case 2:
                                temp = new float[] { 1, 3, 3 };
                                break;
                        }
                        break;
                    case 1:
                        switch (posx)
                        {
                            case 0:
                                temp = new float[] { 2, 2, -3, 2, 2 };
                                break;
                            case 1:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { -2, -3 };
                                        break;
                                    case 2:
                                        temp = new float[] { 2, -3, -2 };
                                        break;
                                }
                                break;
                            case 2:
                                temp = new float[] { -3 };
                                break;
                        }
                        break;
                    case 2:
                        switch (posx)
                        {
                            case 0:
                                temp = new float[] { 6, 2, -6 };
                                break;
                            case 1:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { 3, 6, 2, -6 };
                                        break;
                                    case 2:
                                        temp = new float[] { 3, -6, -4, 6 };
                                        break;
                                }
                                break;
                            case 2:
                                temp = new float[] { -6, -4, 6 };
                                break;
                        }
                        break;
                    case 3:
                        switch (posx)
                        {
                            case 0:
                                temp = new float[] { 3 };
                                break;
                            case 1:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { 4, 3 };
                                        break;
                                    case 2:
                                        temp = new float[] { -4, 3, 4 };
                                        break;
                                }
                                break;
                            case 2:
                                temp = new float[] { 4, 4, 3, 4, 4 };
                                break;
                        }
                        break;
                    case 4:
                        switch (posx)
                        {
                            case 0:
                                temp = new float[] { 6, -2, -6 };
                                break;
                            case 1:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { 5, 6, -2, -6 };
                                        break;
                                    case 2:
                                        temp = new float[] { 5, -6, 4, 6 };
                                        break;
                                }
                                break;
                            case 2:
                                temp = new float[] { -6, 4, 6 };
                                break;
                        }
                        break;
                }
                break;
            case "G":
                switch (side)
                {
                    case 0:
                        switch (posx)
                        {
                            case 0:
                                temp = new float[] { 1, 1, 4, 4 };
                                break;
                            case 1:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { -1, 4, 4 };
                                        break;
                                    case 2:
                                        temp = new float[] { 1, 4, 4 };
                                        break;
                                }
                                break;
                            case 2:
                                temp = new float[] { 4, 4 };
                                break;
                        }
                        break;
                    case 1:
                        switch (posx)
                        {
                            case 0:
                                temp = new float[] { -6, -5, 6 };
                                break;
                            case 1:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { 2, -6, -5, 6 };
                                        break;
                                    case 2:
                                        temp = new float[] { 2, 6, -3, -6 };
                                        break;
                                }
                                break;
                            case 2:
                                temp = new float[] { 6, -3, -6 };
                                break;
                        }
                        break;
                    case 2:
                        switch (posx)
                        {
                            case 0:
                                temp = new float[] { 3, 3, -4, 3, 3 };
                                break;
                            case 1:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { -3, -4 };
                                        break;
                                    case 2:
                                        temp = new float[] { 3, -4, -3 };
                                        break;
                                }
                                break;
                            case 2:
                                temp = new float[] { -4 };
                                break;
                        }
                        break;
                    case 3:
                        switch (posx)
                        {
                            case 0:
                                temp = new float[] { 6, 3, -6 };
                                break;
                            case 1:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { 4, 6, 3, -6 };
                                        break;
                                    case 2:
                                        temp = new float[] { 4, -6, 5, 6 };
                                        break;
                                }
                                break;
                            case 2:
                                temp = new float[] { -6, 5, 6 };
                                break;
                        }
                        break;
                    case 4:
                        switch (posx)
                        {
                            case 0:
                                temp = new float[] { 5, 5, 4, 5, 5 };
                                break;
                            case 1:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { -5, 4 };
                                        break;
                                    case 2:
                                        temp = new float[] { 5, 4, -5 };
                                        break;
                                }
                                break;
                            case 2:
                                temp = new float[] { 4 };
                                break;
                        }
                        break;
                }
                break;
            case "O":
                switch (side)
                {
                    case 0:
                        switch (posx)
                        {
                            case 0:
                                temp = new float[] { 1, 5, 5 };
                                break;
                            case 1:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { 1, 1, 5, 5 };
                                        break;
                                    case 2:
                                        temp = new float[] { 5, 5 };
                                        break;
                                }
                                break;
                            case 2:
                                temp = new float[] { -1, 5, 5 };
                                break;
                        }
                        break;
                    case 1:
                        switch (posx)
                        {
                            case 0:
                                temp = new float[] { -5 };
                                break;
                            case 1:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { 2, -5 };
                                        break;
                                    case 2:
                                        temp = new float[] { -2, -5, 2 };
                                        break;
                                }
                                break;
                            case 2:
                                temp = new float[] { 2, 2, -5, 2, 2 };
                                break;
                        }
                        break;
                    case 2:
                        switch (posx)
                        {
                            case 0:
                                temp = new float[] { -6, 2, 6 };
                                break;
                            case 1:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { 3, -6, 2, 6 };
                                        break;
                                    case 2:
                                        temp = new float[] { 3, 6, -4, -6 };
                                        break;
                                }
                                break;
                            case 2:
                                temp = new float[] { 6, -4, -6 };
                                break;
                        }
                        break;
                    case 3:
                        switch (posx)
                        {
                            case 0:
                                temp = new float[] { 4, 4, 5, 4, 4 };
                                break;
                            case 1:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { -4, 5 };
                                        break;
                                    case 2:
                                        temp = new float[] { 4, 5, -4 };
                                        break;
                                }
                                break;
                            case 2:
                                temp = new float[] { 5 };
                                break;
                        }
                        break;
                    case 4:
                        switch (posx)
                        {
                            case 0:
                                temp = new float[] { -6, -2, 6 };
                                break;
                            case 1:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { 5,- 6, -2, 6 };
                                        break;
                                    case 2:
                                        temp = new float[] { 5, 6, 4, -6 };
                                        break;
                                }
                                break;
                            case 2:
                                temp = new float[] { 6, 4, -6 };
                                break;
                        }
                        break;
                }
                break;
        }

        Finish:
        return temp;
    }

    private float[] BottomCornersMethod()
    {
        float[] temp = new float[] { 0 };
        int posx = 0;
        int posy = 0;
        int side = 0;
        string adColor = "";

        for (int i = 0; i < 5; i++)
        {
            for (int k = 0; k < 9; k += 2)
            {
                if (CurrentColors[k % 3, Mathf.FloorToInt(k / 3), i] == "W")
                {
                    posx = k % 3;
                    posy = Mathf.FloorToInt(k / 3);
                    side = i;
                    goto SkipFind;
                }
            }
        }
        if (CurrentColors[2, 0, 1] + CurrentColors[0, 0, 2] != "BR")
            temp = new float[] { 3, 1, -3 };
        else if (CurrentColors[2, 0, 2] + CurrentColors[0, 0, 3] != "RG")
            temp = new float[] { 4, 1, -4 };
        else if (CurrentColors[2, 0, 3] + CurrentColors[2, 0, 4] != "GO")
            temp = new float[] { -5, 1, 5 };
        else if (CurrentColors[0, 0, 4] + CurrentColors[0, 0, 1] != "OB")
            temp = new float[] { 2, 1, -2 };
        else
            BottomCorners = true;
        goto Finish;
        SkipFind:
        adColor = FindAdjacentCorners(posx, posy, side).Remove(1);

        switch (adColor)
        {
            case "B":
                switch (side)
                {
                    case 0:
                        switch (posx)
                        {
                            case 0:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { 3, -1, -3, -2, 1, 1, 2 };
                                        break;
                                    case 2:
                                        temp = new float[] { 3, 1, 1, -3, -2, -1, 2 };
                                        break;
                                }
                                break;
                            case 2:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { 2, 1, -2, 3, -1, -3 };
                                        break;
                                    case 2:
                                        temp = new float[] { 3, 1, -3, -2, -1, 2 };
                                        break;
                                }
                                break;
                        }
                        break;
                    case 1:
                        switch (posx)
                        {
                            case 0:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { 5, 1, -5, -1, 3, 1, -3 };
                                        break;
                                    case 2:
                                        temp = new float[] { -1, 3, 1, -3 };
                                        break;
                                }
                                break;
                            case 2:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { -2, -1, 2, 1, -2, -1, 2 };
                                        break;
                                    case 2:
                                        temp = new float[] { -2, -1, 2 };
                                        break;
                                }
                                break;
                        }
                        break;
                    case 2:
                        switch (posx)
                        {
                            case 0:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { 3, 1, -3, -1, 3, 1, -3 };
                                        break;
                                    case 2:
                                        temp = new float[] { 3, 1, -3 };
                                        break;
                                }
                                break;
                            case 2:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { 4, -1, -4, 1, -2, -1, 2 };
                                        break;
                                    case 2:
                                        temp = new float[] { 1, -2, -1, 2 };
                                        break;
                                }
                                break;
                        }
                        break;
                    case 3:
                        switch (posx)
                        {
                            case 0:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { 4, 1, -4, 3, 1, -3 };
                                        break;
                                    case 2:
                                        temp = new float[] { 1, 3, 1, -3 };
                                        break;
                                }
                                break;
                            case 2:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { -4, -1, 4, -1, -2, -1, 2 };
                                        break;
                                    case 2:
                                        temp = new float[] { 3, 1, 1, -3 };
                                        break;
                                }
                                break;
                        }
                        break;
                    case 4:
                        switch (posx)
                        {
                            case 0:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { 5, 3, -1, -5, -3 };
                                        break;
                                    case 2:
                                        temp = new float[] { 3, -1, -3 };
                                        break;
                                }
                                break;
                            case 2:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { -5, 1, 1, 5, 3, 1, -3 };
                                        break;
                                    case 2:
                                        temp = new float[] { 1, 1, 3, 1, -3 };
                                        break;
                                }
                                break;
                        }
                        break;
                }
                break;
            case "R":
                switch (side)
                {
                    case 0:
                        switch (posx)
                        {
                            case 0:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { 4, 1, 1, -4, -3, -1, 3 };
                                        break;
                                    case 2:
                                        temp = new float[] { 4, 1, -4, -3, -1, 3 };
                                        break;
                                }
                                break;
                            case 2:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { -3, 1, 3, 4, 1, 1, -4 };
                                        break;
                                    case 2:
                                        temp = new float[] { -1, 4, 1, -4, -3, -1, 3 };
                                        break;
                                }
                                break;
                        }
                        break;
                    case 1:
                        switch (posx)
                        {
                            case 0:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { 2, 1, 1, -2, 4, 1, -4 };
                                        break;
                                    case 2:
                                        temp = new float[] { 1, 1, 4, 1, -4 };
                                        break;
                                }
                                break;
                            case 2:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { -2, -1, 2, -3, -1, 3 };
                                        break;
                                    case 2:
                                        temp = new float[] { 4, -1, -4 };
                                        break;
                                }
                                break;
                        }
                        break;
                    case 2:
                        switch (posx)
                        {
                            case 0:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { -2, 1, 2, -1, 4, 1, -4 };
                                        break;
                                    case 2:
                                        temp = new float[] { -1, 4, 1, -4 };
                                        break;
                                }
                                break;
                            case 2:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { -3, -1, 3, 1, -3, -1, 3 };
                                        break;
                                    case 2:
                                        temp = new float[] { 1, 4, -1, -4 };
                                        break;
                                }
                                break;
                        }
                        break;
                    case 3:
                        switch (posx)
                        {
                            case 0:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { 4, 1, -4, -1, 4, 1, -4 };
                                        break;
                                    case 2:
                                        temp = new float[] { 4, 1, -4 };
                                        break;
                                }
                                break;
                            case 2:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { -5, -1, 5, 1, -3, -1, 3 };
                                        break;
                                    case 2:
                                        temp = new float[] { 1, -3, -1, 3 };
                                        break;
                                }
                                break;
                        }
                        break;
                    case 4:
                        switch (posx)
                        {
                            case 0:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { 5, -1, -5, -1, -3, -1, 3 };
                                        break;
                                    case 2:
                                        temp = new float[] { 4, 1, 1, -4 };
                                        break;
                                }
                                break;
                            case 2:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { -4, 1, 4, -3, 1, 3 };
                                        break;
                                    case 2:
                                        temp = new float[] { -3, 1, 3 };
                                        break;
                                }
                                break;
                        }
                        break;
                }
                break;
            case "G":
                switch (side)
                {
                    case 0:
                        switch (posx)
                        {
                            case 0:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { -5, 1, 5, -4, -1, 4 };
                                        break;
                                    case 2:
                                        temp = new float[] { -1, -5, 1, 5, -4, -1, 4 };
                                        break;
                                }
                                break;
                            case 2:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { -5, 1, 1, 5, -4, -1, 4 };
                                        break;
                                    case 2:
                                        temp = new float[] { -4, 1, 4, -5, 1, 1, 5 };
                                        break;
                                }
                                break;
                        }
                        break;
                    case 1:
                        switch (posx)
                        {
                            case 0:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { 2, 1, -2, -5, 1, 5 };
                                        break;
                                    case 2:
                                        temp = new float[] { 1, -5, 1, 5 };
                                        break;
                                }
                                break;
                            case 2:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { -2, 1, 1, 2, -4, -1, 4 };
                                        break;
                                    case 2:
                                        temp = new float[] { -5, 1, 1, 5};
                                        break;
                                }
                                break;
                        }
                        break;
                    case 2:
                        switch (posx)
                        {
                            case 0:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { 3, 1, 1, -3, -5, 1, 5 };
                                        break;
                                    case 2:
                                        temp = new float[] { -4, 1, 1, 4 };
                                        break;
                                }
                                break;
                            case 2:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { -5, -3, -1, 5, 3 };
                                        break;
                                    case 2:
                                        temp = new float[] { -5, -1, 5 };
                                        break;
                                }
                                break;
                        }
                        break;
                    case 3:
                        switch (posx)
                        {
                            case 0:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { 4, 1, 4, 4, 1, 1, 4 };
                                        break;
                                    case 2:
                                        temp = new float[] { -1, -5, 1, 5 };
                                        break;
                                }
                                break;
                            case 2:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { -4, -1, 4, 1, -4, -1, 4 };
                                        break;
                                    case 2:
                                        temp = new float[] { -4, -1, 4 };
                                        break;
                                }
                                break;
                        }
                        break;
                    case 4:
                        switch (posx)
                        {
                            case 0:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { 5, -1, 5, 5, 1, 1, 5 };
                                        break;
                                    case 2:
                                        temp = new float[] { 1, -4, -1, 4 };
                                        break;
                                }
                                break;
                            case 2:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { -5, 1, 5, -1, -5, 1, 5 };
                                        break;
                                    case 2:
                                        temp = new float[] { -5, 1, 5 };
                                        break;
                                }
                                break;
                        }
                        break;
                }
                break;
            case "O":
                switch (side)
                {
                    case 0:
                        switch (posx)
                        {
                            case 0:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { 5, 1, 1, -5, 2, 1, -2 };
                                        break;
                                    case 2:
                                        temp = new float[] { 5, 1, -5, 2, 1, 1, -2 };
                                        break;
                                }
                                break;
                            case 2:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { 2, 1, -2, 5, -1, -5 };
                                        break;
                                    case 2:
                                        temp = new float[] { 2, 1, 1, -2, 5, -1, -5 };
                                        break;
                                }
                                break;
                        }
                        break;
                    case 1:
                        switch (posx)
                        {
                            case 0:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { 2, 1, -2, -1, 2, 1, -2 };
                                        break;
                                    case 2:
                                        temp = new float[] { 2, 1, -2 };
                                        break;
                                }
                                break;
                            case 2:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { -2, -1, 2, 2, 1, 1, -2 };
                                        break;
                                    case 2:
                                        temp = new float[] { 1, 5, -1, -5 };
                                        break;
                                }
                                break;
                        }
                        break;
                    case 2:
                        switch (posx)
                        {
                            case 0:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { 5, 3, 1, -5, -3 };
                                        break;
                                    case 2:
                                        temp = new float[] { 5, 1, -5 };
                                        break;
                                }
                                break;
                            case 2:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { -3, 1, 1, 3, 5, -1, -5 };
                                        break;
                                    case 2:
                                        temp = new float[] { 2, 1, 1, -2 };
                                        break;
                                }
                                break;
                        }
                        break;
                    case 3:
                        switch (posx)
                        {
                            case 0:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { 4, 1, -4, 5, 1, -5 };
                                        break;
                                    case 2:
                                        temp = new float[] { 5, 1, 1, -5 };
                                        break;
                                }
                                break;
                            case 2:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { -4, -1, 4, 5, -1, -5 };
                                        break;
                                    case 2:
                                        temp = new float[] { 2, -1, -2 };
                                        break;
                                }
                                break;
                        }
                        break;
                    case 4:
                        switch (posx)
                        {
                            case 0:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { 5, -1, -5, 1, 5, -1, -5 };
                                        break;
                                    case 2:
                                        temp = new float[] { 5, -1, -5 };
                                        break;
                                }
                                break;
                            case 2:
                                switch (posy)
                                {
                                    case 0:
                                        temp = new float[] { -5, 1, 5, 5, 1, 1, -5 };
                                        break;
                                    case 2:
                                        temp = new float[] { -1, 2, 1, -2 };
                                        break;
                                }
                                break;
                        }
                        break;
                }
                break;
        }

        Finish:
        return temp;
    }

    private float[] MiddleEdgesMethod()
    {
        float[] temp = new float[] { 0 };
        int posx = 0;
        int posy = 0;
        int side = 0;
        string color = "";
        string adColor = "";

        for (int i = 0; i < 5; i++)
        {
            for (int k = 7; k > 0; k -= 2)
            {
                string currentTile = CurrentColors[k % 3, Mathf.FloorToInt(k / 3), i];

                if (i > 0 && k < 7)
                    break;
                else if (currentTile == "Y" || currentTile == "R" || currentTile == "O")
                    continue;
                else if (FindAdjacentEdge(k % 3, Mathf.FloorToInt(k / 3), i) == "Y")
                    continue;

                posx = k % 3;
                posy = Mathf.FloorToInt(k / 3);
                side = i;
                color = currentTile;
                adColor = FindAdjacentEdge(posx, posy, side);
                goto SkipFind;
            }
        }
        if (CurrentColors[0, 1, 1] != "B" || CurrentColors[0, 1, 4] != "O")
            temp = new float[] { 2, 1, -2, -1, 5, -1, -5 };
        else if (CurrentColors[2, 1, 1] != "B" || CurrentColors[0, 1, 2] != "R")
            temp = new float[] { -2, -1, 2, 1, 3, 1, -3 };
        else if (CurrentColors[0, 1, 3] != "G" || CurrentColors[2, 1, 2] != "R")
            temp = new float[] { 4, 1, -4, -1, -3, -1, 3 };
        else if (CurrentColors[2, 1, 3] != "G" || CurrentColors[2, 1, 4] != "O")
            temp = new float[] { -4, -1, 4, 1, -5, 1, 5 };
        else
            MiddleEdges = true;

        goto Finish;
        SkipFind:

        switch (color)
        {
            case "B":
                switch (adColor)
                {
                    case "O":
                        switch (side)
                        {
                            case 0:
                                switch (posx)
                                {
                                    case 0:
                                        temp = new float[] { 1, 1, 2, 1, -2, -1, 5, -1, -5 };
                                        break;
                                    case 1:
                                        switch (posy)
                                        {
                                            case 0:
                                                temp = new float[] { -1, 2, 1, -2, -1, 5, -1, -5 };
                                                break;
                                            case 2:
                                                temp = new float[] { 1, 2, 1, -2, -1, 5, -1, -5 };
                                                break;
                                        }
                                        break;
                                    case 2:
                                        temp = new float[] { 2, 1, -2, -1, 5, -1, -5 };
                                        break;
                                }
                                break;
                            case 1:
                                temp = new float[] { -1, 5, -1, -5, 1, 2, 1, -2 };
                                break;
                            case 2:
                                temp = new float[] { 5, -1, -5, 1, 2, 1, -2 };
                                break;
                            case 3:
                                temp = new float[] { 1, 5, -1, -5, 1, 2, 1, -2 };
                                break;
                            case 4:
                                temp = new float[] { 1, 1, 5, -1, -5, 1, 2, 1, -2 };
                                break;
                        }
                        break;
                    case "R":
                        switch (side)
                        {
                            case 0:
                                switch (posx)
                                {
                                    case 0:
                                        temp = new float[] { 1, 1, -2, -1, 2, 1, 3, 1, -3 };
                                        break;
                                    case 1:
                                        switch (posy)
                                        {
                                            case 0:
                                                temp = new float[] { -1, -2, -1, 2, 1, 3, 1, -3 };
                                                break;
                                            case 2:
                                                temp = new float[] { 1, -2, -1, 2, 1, 3, 1, -3 };
                                                break;
                                        }
                                        break;
                                    case 2:
                                        temp = new float[] { -2, -1, 2, 1, 3, 1, -3 };
                                        break;
                                }
                                break;
                            case 1:
                                temp = new float[] { 1, 3, 1, -3, -1, -2, -1, 2 };
                                break;
                            case 2:
                                temp = new float[] { 1, 1, 3, 1, -3, -1, -2, -1, 2 };
                                break;
                            case 3:
                                temp = new float[] { -1, 3, 1, -3, -1, -2, -1, 2 };
                                break;
                            case 4:
                                temp = new float[] { 3, 1, -3, -1, -2, -1, 2 };
                                break;
                        }
                        break;
                }
                break;
            case "G":
                switch (adColor)
                {
                    case "R":
                        switch (side)
                        {
                            case 0:
                                switch (posx)
                                {
                                    case 0:
                                        temp = new float[] { 4, 1, -4, -1, -3, -1, 3 };
                                        break;
                                    case 1:
                                        switch (posy)
                                        {
                                            case 0:
                                                temp = new float[] { 1, 4, 1, -4, -1, -3, -1, 3 };
                                                break;
                                            case 2:
                                                temp = new float[] { -1, 4, 1, -4, -1, -3, -1, 3 };
                                                break;
                                        }
                                        break;
                                    case 2:
                                        temp = new float[] { 1, 1, 4, 1, -4, -1, -3, -1, 3 };
                                        break;
                                }
                                break;
                            case 1:
                                temp = new float[] { 1, -3, -1, 3, 1, 4, 1, -4 };
                                break;
                            case 2:
                                temp = new float[] { 1, 1, -3, -1, 3, 1, 4, 1, -4 };
                                break;
                            case 3:
                                temp = new float[] { -1, -3, -1, 3, 1, 4, 1, -4 };
                                break;
                            case 4:
                                temp = new float[] { -3, -1, 3, 1, 4, 1, -4 };
                                break;
                        }
                        break;
                    case "O":
                        switch (side)
                        {
                            case 0:
                                switch (posx)
                                {
                                    case 0:
                                        temp = new float[] { -4, -1, 4, 1, -5, 1, 5 };
                                        break;
                                    case 1:
                                        switch (posy)
                                        {
                                            case 0:
                                                temp = new float[] { 1, -4, -1, 4, 1, -5, 1, 5 };
                                                break;
                                            case 2:
                                                temp = new float[] { -1, -4, -1, 4, 1, -5, 1, 5 };
                                                break;
                                        }
                                        break;
                                    case 2:
                                        temp = new float[] { 1, 1, -4, -1, 4, 1, -5, 1, 5 };
                                        break;
                                }
                                break;
                            case 1:
                                temp = new float[] { -1, -5, 1, 5, -1, -4, -1, 4 };
                                break;
                            case 2:
                                temp = new float[] { -5, 1, 5, -1, -4, -1, 4 };
                                break;
                            case 3:
                                temp = new float[] { 1, -5, 1, 5, -1, -4, -1, 4 };
                                break;
                            case 4:
                                temp = new float[] { 1, 1, -5, 1, 5, -1, -4, -1, 4 };
                                break;
                        }
                        break;
                }
                break;
        }

        Finish:
        return temp;
    }

    private float[] TopCrossMethod()
    {
        float[] temp = new float[] { 0 };
        int side = 0;
        int biggest = 0;

        if (CurrentColors[0, 1, 0] + CurrentColors[1, 0, 0] + CurrentColors[1, 2, 0] + CurrentColors[2, 1, 0] == "YYYY")
        {
            TopCross = true;
            goto Finish;
        }

        for (int i = 0; i < 4; i++)
        {
            string[] line = new string[3];
            int consecutive;
            for (int j = 0; j < 3; j++)
            {
                line[j] = CurrentColors[(int)Math.Sign((4 - (3 * i + j)) * ((3 * i + j) - 10)) + 1, (int)Math.Sign(((3 * i + j) - 1) * ((3 * i + j) - 7)) + 1, 0];
            }
            switch (line[0])
            {
                case "Y":
                    switch (line[1])
                    {
                        case "Y":
                            switch (line[2])
                            {
                                case "Y":
                                    consecutive = 0;
                                    break;
                                default:
                                    consecutive = 1;
                                    break;
                            }
                            break;
                        default:
                            switch (line[2])
                            {
                                case "Y":
                                    consecutive = 1;
                                    break;
                                default:
                                    consecutive = 2;
                                    break;
                            }
                            break;
                    }
                    break;
                default:
                    switch (line[1])
                    {
                        case "Y":
                            consecutive = 1;
                            break;
                        default:
                            switch (line[2])
                            {
                                case "Y":
                                    consecutive = 2;
                                    break;
                                default:
                                    consecutive = 3;
                                    break;
                            }
                            break;
                    }
                    break;
            }
            if (consecutive > biggest)
            {
                biggest = consecutive;
                side = i;
            }
        }

        switch (side)
        {
            case 0:
                temp = new float[] { 2, 1, 3, -1, -3, -2 };
                break;
            case 1:
                temp = new float[] { 3, 1, 4, -1, -4, -3 };
                break;
            case 2:
                temp = new float[] { 4, 1, -5, -1, 5, -4 };
                break;
            case 3:
                temp = new float[] { -5, 1, 2, -1, -2, 5 };
                break;
        }

        Finish:
        return temp;
    }

    private float[] TopCornersMethod()
    {
        float[] temp = new float[] { 0 };
        int[] corners = new int[3];

        for (int i = 0; i < 3; i++)
        {
            string temporary  = CurrentColors[2 * (int)Math.Sign(i * (i - 1)), 2 * (int)Math.Sign((i - 1) * (i - 2)), 0] + FindAdjacentCorners(2 * (int)Math.Sign(i * (i - 1)), 2 * (int)Math.Sign((i - 1) * (i - 2)), 0);
            corners[i] = temporary.IndexOf("Y");
        }

        switch (corners[0])
        {
            case 0:
                switch (corners[1])
                {
                    case 0:
                        switch (corners[2])
                        {
                            case 0:
                                TopCorners = true;
                                break;
                            default:
                                temp = new float[] { -5, 1, 5, 1, -5, 1, 1, 5 };
                                break;
                        }
                        break;
                    case 1:
                        switch (corners[2])
                        {
                            case 2:
                                temp = new float[] { 4, 1, -4, 1, 4, 1, 1, -4 };
                                break;
                            default:
                                temp = new float[] { 3, 1, -3, 1, 3, 1, 1, -3 };
                                break;
                        }
                        break;
                    case 2:
                        switch (corners[2])
                        {
                            case 0:
                                temp = new float[] { -5, 1, 5, 1, -5, 1, 1, 5 };
                                break;
                            case 1:
                                temp = new float[] { 4, 1, -4, 1, 4, 1, 1, -4 };
                                break;
                            case 2:
                                temp = new float[] { 3, 1, -3, 1, 3, 1, 1, -3 };
                                break;
                        }
                        break;
                }
                break;
            case 1:
                switch (corners[1])
                {
                    case 0:
                        switch (corners[2])
                        {
                            case 1:
                                temp = new float[] { 4, 1, -4, 1, 4, 1, 1, -4 };
                                break;
                            default:
                                temp = new float[] { 2, 1, -2, 1, 2, 1, 1, -2 };
                                break;
                        }
                        break;
                    case 1:
                        switch (corners[2])
                        {
                            case 0:
                                temp = new float[] { -5, 1, 5, 1, -5, 1, 1, 5 };
                                break;
                            default:
                                temp = new float[] { 2, 1, -2, 1, 2, 1, 1, -2 };
                                break;
                        }
                        break;
                    case 2:
                        switch (corners[2])
                        {
                            case 2:
                                temp = new float[] { -5, 1, 5, 1, -5, 1, 1, 5 };
                                break;
                            default:
                                temp = new float[] { 3, 1, -3, 1, 3, 1, 1, -3 };
                                break;
                        }
                        break;
                }
                break;
            case 2:
                switch (corners[1])
                {
                    case 0:
                        switch (corners[2])
                        {
                            case 0:
                                temp = new float[] { 2, 1, -2, 1, 2, 1, 1, -2 };
                                break;
                            default:
                                temp = new float[] { 4, 1, -4, 1, 4, 1, 1, -4 };
                                break;
                        }
                        break;
                    case 1:
                        switch (corners[2])
                        {
                            case 2:
                                temp = new float[] { 4, 1, -4, 1, 4, 1, 1, -4 };
                                break;
                            default:
                                temp = new float[] { 3, 1, -3, 1, 3, 1, 1, -3 };
                                break;
                        }
                        break;
                    case 2:
                        switch (corners[2])
                        {
                            case 0:
                                temp = new float[] { -5, 1, 5, 1, -5, 1, 1, 5 };
                                break;
                            case 1:
                                temp = new float[] { 4, 1, -4, 1, 4, 1, 1, -4 };
                                break;
                            case 2:
                                temp = new float[] { 2, 1, -2, 1, 2, 1, 1, -2 };
                                break;
                        }
                        break;
                }
                break;
        }

        return temp;
    }

    private float[] TopCornersPermutationMethod()
    {
        float[] temp = new float[] { 0 };
        string corners = "";

        for (int i = 0; i < 4; i++)
        {
            corners += FindAdjacentCorners(2 * (int)Math.Sign(i * (i - 1)), 2 * (int)Math.Sign((i - 1) * (i - 2)), 0) + " ";
        }

        switch (corners.IndexOf("BO "))
        {
            case 0:
                switch (corners.IndexOf("RB "))
                {
                    case 3:
                        switch (corners.IndexOf("GR "))
                        {
                            case 6:
                                TopCornersPermutation = true;
                                break;
                            default:
                                temp = new float[] { 5, 4, 5, 2, 2, -5, -4, 5, 2, 2, 5, 5 };
                                break;
                        }
                        break;
                    case 6:
                        switch (corners.IndexOf("GR "))
                        {
                            case 3:
                                temp = new float[] { -4, 3, -4, 5, 5, 4, -3, -4, 5, 5, 4, 4 };
                                break;
                            case 9:
                                temp = new float[] { -3, 2, -3, 4, 4, 3, -2, -3, 4, 4, 3, 3 };
                                break;
                        }
                            break;
                    case 9:
                        switch (corners.IndexOf("GR "))
                        {
                            case 3:
                                temp = new float[] { -2, -5, -2, 3, 3, 2, 5, -2, 3, 3, 2, 2 };
                                break;
                            case 6:
                                temp = new float[] { -3, 2, -3, 4, 4, 3, -2, -3, 4, 4, 3, 3 };
                                break;
                        }
                        break;
                }
                break;
            case 3:
                switch (corners.IndexOf("RB "))
                {
                    case 0:
                        switch (corners.IndexOf("GR "))
                        {
                            case 6:
                                temp = new float[] { -3, 2, -3, 4, 4, 3, -2, -3, 4, 4, 3, 3 };
                                break;
                            case 9:
                                temp = new float[] { -4, 3, -4, 5, 5, 4, -3, -4, 5, 5, 4, 4 };
                                break;
                        }
                        break;
                    case 6:
                        switch (corners.IndexOf("GR "))
                        {
                            case 9:
                                TopCornersPermutation = true;
                                break;
                            default:
                                temp = new float[] { -2, -5, -2, 3, 3, 2, 5, -2, 3, 3, 2, 2 };
                                break;
                        }
                        break;
                    case 9:
                        switch (corners.IndexOf("GR "))
                        {
                            case 0:
                                temp = new float[] { -4, 3, -4, 5, 5, 4, -3, -4, 5, 5, 4, 4 };
                                break;
                            case 6:
                                temp = new float[] { 5, 4, 5, 2, 2, -5, -4, 5, 2, 2, 5, 5 };
                                break;
                        }
                        break;
                }
                break;
            case 6:
                switch (corners.IndexOf("RB "))
                {
                    case 0:
                        switch (corners.IndexOf("GR "))
                        {
                            case 3:
                                temp = new float[] { 5, 4, 5, 2, 2, -5, -4, 5, 2, 2, 5, 5 };
                                break;
                            case 9:
                                temp = new float[] { -2, -5, -2, 3, 3, 2, 5, -2, 3, 3, 2, 2 };
                                break;
                        }
                        break;
                    case 3:
                        switch (corners.IndexOf("GR "))
                        {
                            case 0:
                                temp = new float[] { -4, 3, -4, 5, 5, 4, -3, -4, 5, 5, 4, 4 };
                                break;
                            case 9:
                                temp = new float[] { -4, 3, -4, 5, 5, 4, -3, -4, 5, 5, 4, 4 };
                                break;
                        }
                        break;
                    case 9:
                        switch (corners.IndexOf("GR "))
                        {
                            case 0:
                                TopCornersPermutation = true;
                                break;
                            default:
                                temp = new float[] { -3, 2, -3, 4, 4, 3, -2, -3, 4, 4, 3, 3 };
                                break;
                        }
                        break;
                }
                break;
            case 9:
                switch (corners.IndexOf("RB "))
                {
                    case 0:
                        switch (corners.IndexOf("GR "))
                        {
                            case 3:
                                TopCornersPermutation = true;
                                break;
                            default:
                                temp = new float[] { -4, 3, -4, 5, 5, 4, -3, -4, 5, 5, 4, 4 };
                                break;
                        }
                        break;
                    case 3:
                        switch (corners.IndexOf("GR "))
                        {
                            case 0:
                                temp = new float[] { -3, 2, -3, 4, 4, 3, -2, -3, 4, 4, 3, 3 };
                                break;
                            case 6:
                                temp = new float[] { -2, -5, -2, 3, 3, 2, 5, -2, 3, 3, 2, 2 };
                                break;
                        }
                        break;
                    case 6:
                        switch (corners.IndexOf("GR "))
                        {
                            case 0:
                                temp = new float[] { 5, 4, 5, 2, 2, -5, -4, 5, 2, 2, 5, 5 };
                                break;
                            case 3:
                                temp = new float[] { -4, 3, -4, 5, 5, 4, -3, -4, 5, 5, 4, 4 };
                                break;
                        }
                        break;
                }
                break;
        }

        return temp;
    }

    private float[] FewTouchesMethod()
    {
        float[] temp = new float[] { 0 };
        string indicators = CurrentColors[0, 2, 1];

        switch (indicators)
        {
            case "B":
                FewTouches = true;
                break;
            case "R":
                temp = new float[] { -1 };
                break;
            case "G":
                temp = new float[] { 1, 1 };
                break;
            case "O":
                temp = new float[] { 1 };
                break;
        }

        return temp;
    }

    private float[] TopEdgesPermutationMethod()
    {
        float[] temp = new float[] { 0 };
        string[] edges = new string[2];

        for (int i = 0; i < 2; i++)
        {
            edges[i] = CurrentColors[1, 2, i + 1];
        }

        switch (edges[0])
        {
            case "B":
                switch (edges[1])
                {
                    case "R":
                        TopEdgesPermutation = true;
                        break;
                    case "G":
                        temp = new float[] { 4, 4, -1, 3, 5, 4, 4, -3, -5, -1, 4, 4 };
                        break;
                    case "O":
                        temp = new float[] { 4, 4, 1, 3, 5, 4, 4, -3, -5, 1, 4, 4 };
                        break;
                }
                break;
            case "R":
                switch (edges[1])
                {
                    case "B":
                        temp = new float[] { 3, 3, 1, 2, -4, 3, 3, -2, 4, 1, 3, 3 };
                        break;
                    case "G":
                        temp = new float[] { 3, 3, -1, 2, -4, 3, 3, -2, 4, -1, 3, 3 };
                        break;
                    case "O":
                        temp = new float[] { 2, 2, -1, -5, -3, 2, 2, 5, 3, -1, 2, 2 };
                        break;
                }
                break;
            case "G":
                switch (edges[1])
                {
                    case "B":
                        temp = new float[] { 3, 3, 1, 2, -4, 3, 3, -2, 4, 1, 3, 3 };
                        break;
                    case "R":
                        temp = new float[] { 5, 5, -1, 4, -2, 5, 5, -4, 2, -1, 5, 5 };
                        break;
                    case "O":
                        temp = new float[] { 3, 3, 1, 2, -4, 3, 3, -2, 4, 1, 3, 3 };
                        break;
                }
                break;
            case "O":
                switch (edges[1])
                {
                    case "B":
                        temp = new float[] { 2, 2, 1, -5, -3, 2, 2, 5, 3, 1, 2, 2 };
                        break;
                    case "R":
                        temp = new float[] { 5, 5, 1, 4, -2, 5, 5, -4, 2, 1, 5, 5 };
                        break;
                    case "G":
                        temp = new float[] { 3, 3, 1, 2, -4, 3, 3, -2, 4, 1, 3, 3 };
                        break;
                }
                break;
        }

        return temp;
    }
}