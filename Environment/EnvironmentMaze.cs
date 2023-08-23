
using Cartheur.Ideal.Mooc.Coupling;
using Cartheur.Ideal.Mooc.Interfaces;

namespace Cartheur.Ideal.Mooc.Environment
{
    public class EnvironmentMaze : Environment50, IEnvironment
    {
        static int ORIENTATION_UP = 0;
        static int ORIENTATION_RIGHT = 1;
        static int ORIENTATION_DOWN = 2;
        static int ORIENTATION_LEFT = 3;
        Existence existence = new Existence();

        // The Small Loop Environment

        static int WIDTH = 6;
        static int HEIGHT = 6;
        int m_x = 4;
        int m_y = 1;
        int m_o = 2;

        private char[] m_board =
            {
         {'x', 'x', 'x', 'x', 'x', 'x'},
         {'x', ' ', ' ', ' ', ' ', 'x'},
         {'x', ' ', 'x', 'x', ' ', 'x'},
         {'x', ' ', ' ', 'x', ' ', 'x'},
         {'x', 'x', ' ', ' ', ' ', 'x'},
         {'x', 'x', 'x', 'x', 'x', 'x'},
        };

        private char[] m_agent =
        { '^', '>', 'v', '<' };

        public EnvironmentMaze(Existence existence, int x, int y, int o, char[] board, char[] agent)
        {
            this.existence = existence;
            m_x = x;
            m_y = y;
            m_o = o;
            m_board = board;
            m_agent = agent;
        }

        protected void Initialize()
        {

            //Settings for a nice demo in the Simple Maze 
            Interaction turnLeft = this.GetExistence().AddOrGetPrimitiveInteraction("^t", -3); // Left toward empty
            Interaction turnRight = this.GetExistence().AddOrGetPrimitiveInteraction("vt", -3); // Right toward empty
            Interaction touchRight = this.GetExistence().AddOrGetPrimitiveInteraction("\\t", -1); // Touch right wall
            this.GetExistence().AddOrGetPrimitiveInteraction("\\f", -1); // Touch right empty
            Interaction touchLeft = this.GetExistence().AddOrGetPrimitiveInteraction("/t", -1); // Touch left wall
            this.GetExistence().AddOrGetPrimitiveInteraction("/f", -1); // Touch left empty
            Interaction froward = this.GetExistence().AddOrGetPrimitiveInteraction(">t", 5); // Move
            this.GetExistence().AddOrGetPrimitiveInteraction(">f", -10); // Bump		
            Interaction touchForward = this.GetExistence().AddOrGetPrimitiveInteraction("-t", -1); // Touch wall
            this.GetExistence().AddOrGetPrimitiveInteraction("-f", -1); // Touch empty
            this.GetExistence().AddOrGetAbstractExperience(turnLeft);
            this.GetExistence().AddOrGetAbstractExperience(turnRight);
            this.GetExistence().AddOrGetAbstractExperience(touchRight);
            this.GetExistence().AddOrGetAbstractExperience(touchLeft);
            this.GetExistence().AddOrGetAbstractExperience(froward);
            this.GetExistence().AddOrGetAbstractExperience(touchForward);
        }

        public Interaction Enact(Interaction intendedInteraction)
        {
            Interaction enactedInteraction = null;

            if (intendedInteraction.GetLabel().Substring(0, 1).Equals(">"))
                enactedInteraction = Move();
            else if (intendedInteraction.GetLabel().Substring(0, 1).Equals("^"))
                enactedInteraction = Left();
            else if (intendedInteraction.GetLabel().Substring(0, 1).Equals("v"))
                enactedInteraction = Right();
            else if (intendedInteraction.GetLabel().Substring(0, 1).Equals("-"))
                enactedInteraction = Touch();
            else if (intendedInteraction.GetLabel().Substring(0, 1).Equals("\\"))
                enactedInteraction = TouchRight();
            else if (intendedInteraction.GetLabel().Substring(0, 1).Equals("/"))
                enactedInteraction = TouchLeft();

            // print the maze
            for (int i = 0; i < HEIGHT; i++)
            {
                for (int j = 0; j < WIDTH; j++)
                {
                    if (i == m_y && j == m_x)
                        Console.WriteLine(m_agent[m_o]);

                else
                        Console.WriteLine(m_board[i][j]);
                }
                Console.WriteLine();
            }

            return enactedInteraction;
        }

        /**
         * Turn to the right. 
         */
        private Interaction Right()
        {
            m_o++;
            if (m_o > ORIENTATION_LEFT)
                m_o = ORIENTATION_UP;

            return this.GetExistence().AddOrGetPrimitiveInteraction("vt", 0);
        }

        /**
         * Turn to the left. 
         */
        private Interaction Left()
        {
            m_o--;
            if (m_o < 0)
                m_o = ORIENTATION_LEFT;

            return this.GetExistence().AddOrGetPrimitiveInteraction("^t", 0);
        }

        /**
         * Move forward to the direction of the current orientation.
         */
        private Interaction Move()
        {
            Interaction enactedInteraction = this.GetExistence().AddOrGetPrimitiveInteraction(">f", 0);

            if ((m_o == ORIENTATION_UP) && (m_y > 0) && (m_board[m_y - 1][m_x] == ' '))
            {
                m_y--;
                enactedInteraction = this.GetExistence().AddOrGetPrimitiveInteraction(">t", 0);
            }

            if ((m_o == ORIENTATION_DOWN) && (m_y < HEIGHT) && (m_board[m_y + 1][m_x] == ' '))
            {
                m_y++;
                enactedInteraction = this.GetExistence().AddOrGetPrimitiveInteraction(">t", 0);
            }

            if ((m_o == ORIENTATION_RIGHT) && (m_x < WIDTH) && (m_board[m_y][m_x + 1] == ' '))
            {
                m_x++;
                enactedInteraction = this.GetExistence().AddOrGetPrimitiveInteraction(">t", 0);
            }
            if ((m_o == ORIENTATION_LEFT) && (m_x > 0) && (m_board[m_y][m_x - 1] == ' '))
            {
                m_x--;
                enactedInteraction = this.GetExistence().AddOrGetPrimitiveInteraction(">t", 0);
            }

            return enactedInteraction;
        }

        /**
         * Touch the square forward.
         * Succeeds if there is a wall, fails otherwise 
         */
        private Interaction Touch()
        {
            Interaction enactedInteraction = this.GetExistence().AddOrGetPrimitiveInteraction("-t", 0);

            if (((m_o == ORIENTATION_UP) && (m_y > 0) && (m_board[m_y - 1][m_x] == ' ')) ||
                ((m_o == ORIENTATION_DOWN) && (m_y < HEIGHT) && (m_board[m_y + 1][m_x] == ' ')) ||
                ((m_o == ORIENTATION_RIGHT) && (m_x < WIDTH) && (m_board[m_y][m_x + 1] == ' ')) ||
                ((m_o == ORIENTATION_LEFT) && (m_x > 0) && (m_board[m_y][m_x - 1] == ' ')))
            {
                enactedInteraction = this.GetExistence().AddOrGetPrimitiveInteraction("-f", 0);
            }

            return enactedInteraction;
        }

        /**
         * Touch the square to the right.
         * Succeeds if there is a wall, fails otherwise. 
         */
        private Interaction TouchRight()
        {
            Interaction enactedInteraction = this.GetExistence().AddOrGetPrimitiveInteraction("\\t", 0);

            if (((m_o == ORIENTATION_UP) && (m_x > 0) && (m_board[m_y][m_x + 1] == ' ')) ||
                ((m_o == ORIENTATION_DOWN) && (m_x < WIDTH) && (m_board[m_y][m_x - 1] == ' ')) ||
                ((m_o == ORIENTATION_RIGHT) && (m_y < HEIGHT) && (m_board[m_y + 1][m_x] == ' ')) ||
                ((m_o == ORIENTATION_LEFT) && (m_y > 0) && (m_board[m_y - 1][m_x] == ' ')))
            {
                enactedInteraction = this.GetExistence().AddOrGetPrimitiveInteraction("\\f", 0);
            }

            return enactedInteraction;
        }

        /**
         * Touch the square forward.
         * Succeeds if there is a wall, fails otherwise 
         */
        private Interaction TouchLeft()
        {
            Interaction enactedInteraction = this.GetExistence().AddOrGetPrimitiveInteraction("/t", 0);

            if (((m_o == ORIENTATION_UP) && (m_x > 0) && (m_board[m_y][m_x - 1] == ' ')) ||
                ((m_o == ORIENTATION_DOWN) && (m_x < WIDTH) && (m_board[m_y][m_x + 1] == ' ')) ||
                ((m_o == ORIENTATION_RIGHT) && (m_y > 0) && (m_board[m_y - 1][m_x] == ' ')) ||
                ((m_o == ORIENTATION_LEFT) && (m_y < HEIGHT) && (m_board[m_y + 1][m_x] == ' ')))
            { enactedInteraction = this.GetExistence().AddOrGetPrimitiveInteraction("/f", 0); }

            return enactedInteraction;
        }
    }
}
