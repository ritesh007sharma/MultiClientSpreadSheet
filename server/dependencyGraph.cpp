#include <algorithm>


#include "dependencyGraph.h"



using namespace std;

DependencyGraph::DependencyGraph()
{

}

DependencyGraph::~DependencyGraph()
{

}

set<string> DependencyGraph::getDependees(string cell) {
    if (dependees.count(cell)) {
        return dependees[cell];
    }
    else {
        set<string> empty;
        return empty;
    }
}

void DependencyGraph::addDependencies(std::string cell, set<std::string> dpndees) {
    

    if (dependees.count(cell)) {
        dependees[cell] = dpndees;
    }
    else {
        dependees.insert(std::make_pair(cell, dpndees));
    }
}


bool DependencyGraph::hasCircularDependency(string startCell, string cell, set<string> dependees, vector<string> &visited)  {
    std::set<string>::iterator it = dependees.begin();

    while (it != dependees.end()) {
        if (!count(visited.begin(), visited.end(), *it)) {
            visited.push_back(*it);

            if (*it == startCell)
                return true;

            if (hasCircularDependency(startCell, *it, getDependees(*it), visited)) {
                return true;
            }
        }
        it++;
    }
    return false;
}