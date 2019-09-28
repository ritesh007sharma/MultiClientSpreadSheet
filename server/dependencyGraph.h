#ifndef DEPENDENCYGRAPH_H
#define DEPENDENCYGRAPH_H

#include <map>
#include <set>
#include <vector>
#include <string>

class DependencyGraph {
    private:
        std::map<std::string, std::set<std::string> > dependees;
    public:
        DependencyGraph();
        ~DependencyGraph();
        std::set<std::string> getDependees(std::string);
        void addDependencies(std::string, std::set<std::string>);
        bool hasCircularDependency(std::string, std::string, std::set<std::string>, std::vector<std::string> &);
};

#endif