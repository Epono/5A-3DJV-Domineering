// Domineering.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <vector>
#include <stack>

class Board;
struct Move
{

public:
	int x, y, h_or_v;


	Move(int p_x, int p_y, int p_h_or_v) : x(p_x), y(p_y), h_or_v( p_h_or_v)
	{
	
	}


	static bool CanMoveBeDone(int p_x, int p_y, int p_h_or_v, Board* board);

	bool CanBeDone(Board* b)
	{
		return CanMoveBeDone(x, y, h_or_v, b);
	}
};


#define INFINI (1 << 30)

class Board
{

public:
	int w, h;
	int  **tab;

	std::stack<Move> movesDone;

	Board(int p_w, int p_h) : w(p_w), h(p_h)
	{
		tab = new int*[p_w];
		for (int i = 0; i < p_w; i++)
		{
			tab[i] = new int[p_h];
			for (int j = 0; j < p_h; j++)
			{
				tab[i][j] = 0;
			}
		}
	}

	Board(const Board& board) : Board(board.w, board.h)
	{
		for (int i = 0; i < w; i++)
		{
			for (int j = 0; j < h; j++)
			{
				tab[i][j] = board.tab[i][j];
			}
		}
	}

	Board operator=(const Board& board)
	{
		movesDone = board.movesDone;
		for (int i = 0; i < w; i++)
		{
			for (int j = 0; j < h; j++)
			{
				tab[i][j] = board.tab[i][j];
			}
		}

		return (*this);
	}
	
	~Board()
	{
		for (int i = 0; i < w; i++)
		{
			delete[] tab[i];
		}

		delete[] tab;
	}
	void DoMove(Move move)
	{
		tab[move.x][move.y] = move.h_or_v + 1;
		tab[move.x + (move.h_or_v == 0 ? 1 : 0)][move.y + (move.h_or_v == 1 ? 1 : 0)] = move.h_or_v + 1;
		movesDone.push(move);
	}

	void UndoMove()
	{
		Move move = movesDone.top();
		
		tab[move.x][move.y] = 0;
		tab[move.x + (move.h_or_v == 0 ? 1 : 0)][move.y + (move.h_or_v == 1 ? 1 : 0)] = 0;

		movesDone.pop();
	}

	Board BoardAfterMove(Move move)
	{
		Board board = *this;
		board.tab[move.x][move.y] = move.h_or_v + 1;
		board.tab[move.x + (move.h_or_v == 0 ? 1 : 0)][move.y + (move.h_or_v == 1 ? 1 : 0)] = move.h_or_v + 1;
		return board;
	}


	std::vector<Move> PossibleMoves(int p_h_or_v)
	{
		std::vector<Move> moves;
		for (int i = 0; i < w; i++)
		{
			for (int j = 0; j < h; j++)
			{
				if (Move::CanMoveBeDone(i, j, p_h_or_v, this))
				{
					moves.push_back(Move(i, j, p_h_or_v));
				}
			}
		}

		return moves;
	}

	bool canPlay(int p_h_or_v)
	{
		for (int i = 0; i < w; i++)
		{
			for (int j = 0; j < h; j++)
			{
				if (Move::CanMoveBeDone(i, j, p_h_or_v, this))
				{
					return true;
				}
			}
		}

		return false;
	}

	int evaluate(int p_h_or_v)
	{
		int coups_possibles_h = 0, coups_possibles_v = 0;
	
		for (int i = 0; i < w; i++)
		{
			for (int j = 0; j < h; j++)
			{
				if (Move::CanMoveBeDone(i, j, 0, this))
				{
					coups_possibles_h++;
				}
				
				if (Move::CanMoveBeDone(i, j, 1, this))
				{
					coups_possibles_v++;
				}
			}
		}

		if (p_h_or_v == 0)
		{
			if (coups_possibles_v == 0)
			{
				return INFINI;
			}

			if (coups_possibles_h == 0)
			{
				return -INFINI;
			}


			return coups_possibles_h - coups_possibles_v;
		}
		else 
		{
			if (coups_possibles_h == 0)
			{
				return INFINI;
			}

			if (coups_possibles_v == 0)
			{
 				return -INFINI;
			}

			return coups_possibles_v - coups_possibles_h;
		}

	}

	Move GetBestMove(int p_h_or_v)
	{	
		std::vector<Move> moves = PossibleMoves(p_h_or_v);

		int best_evaluation = -1;
		int best_evaluation_index = -1;
		for (int i = 0; i < moves.size(); i++)
		{
			this->DoMove(moves[i]);

			int eva = this->evaluate(p_h_or_v);
			if (best_evaluation_index == -1 || eva > best_evaluation)
			{
				best_evaluation = eva;
				best_evaluation_index = i;
			}

			this->UndoMove();

			if (eva == INFINI) // mouvement gagnant
			{
				return moves[i];
			}
		}
	
		return moves[best_evaluation_index];
	}
	
	int NegaMaxScore(int p_h_or_v, int profondeur, int alpha, int beta, bool min = false)
	{
		int eva = evaluate(p_h_or_v);

		if (profondeur == 0)
		{
			return eva;
		}

		std::vector<Move> moves = PossibleMoves(p_h_or_v);

		if (moves.size() == 0)
		{
			return eva;
		}

		int bestMoveIndex = -1;
		int bestMoveScore = -1;

		for (int i = 0; i < moves.size(); i++)
		{
			this->DoMove(moves[i]);
			int moveScore = -this->NegaMaxScore(p_h_or_v, profondeur - 1, -beta, -alpha, !min);

			this->UndoMove();

			if (moveScore == INFINI)
			{
				return moveScore;
			}


			if (bestMoveIndex == -1 || (moveScore > bestMoveScore) )
			{
				bestMoveIndex = i;
				bestMoveScore = moveScore;

				if (bestMoveScore > alpha)
				{
					alpha = bestMoveScore;
					if (alpha > beta)
					{
						return bestMoveScore;
					}
				}

			}
		}

		return bestMoveScore;
	}

	Move MiniMax(int p_h_or_v, int profondeur)
	{
		int bestMoveIndex = -1;
		int bestMoveScore = -1;
		int alpha = -INFINI;
		int beta = INFINI;

		std::vector<Move> moves = PossibleMoves(p_h_or_v);
		for (int i = 0; i < moves.size(); i++)
		{
			this->DoMove(moves[i]);
			int moveScore =  NegaMaxScore(p_h_or_v, profondeur, alpha, beta);
			this->UndoMove();

			if (moveScore == INFINI)
			{
				return moves[i];
			}

			if (bestMoveIndex == -1 || moveScore > bestMoveScore)
			{
				bestMoveIndex = i;
				bestMoveScore = moveScore;
				if (bestMoveScore > alpha)
				{
					alpha = bestMoveScore;
				
					if (alpha > beta)
					{
						return moves[i];
					}
				}
			}
		}

		return moves[bestMoveIndex];
	}

	void Display()
	{
		printf("\n");
		for (int y = 0; y < h; y++)
		{
			for (int x = 0; x < w; x++)
			{
				printf("%c", '0' + tab[x][y]);
			}
			printf("\n");
		}	
	}

};

bool Move::CanMoveBeDone(int p_x, int p_y, int p_h_or_v, Board* board)
{
	int x_plus = (p_h_or_v == 0 ? 1 : 0);
	int y_plus = (p_h_or_v == 1 ? 1 : 0);

	if (p_x < 0 || ((p_x + x_plus ) >= board->w) || p_y < 0 || ((p_y + y_plus) >= board->h))
	{
		return false;
	}

	if (board->tab[p_x][p_y] != 0)
	{
		return false;
	}

	if (board->tab[p_x + x_plus][p_y + y_plus] != 0)
	{
		return false;
	}

	return true;
}


int _tmain(int argc, _TCHAR* argv[])
{
	Board board(8, 8);

	int player = 0;
	
	board.Display();

	while (board.canPlay(player))
	{	
		Move move = board.MiniMax(player, 3 );
		board.DoMove(move);

		board.Display();
		player = player == 0 ? 1 : 0;	
	}

	printf("%s loses\n", player == 0 ? "H" : "V");
	system("pause");

	return 0;
}

