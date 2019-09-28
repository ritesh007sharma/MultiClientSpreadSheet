
#include <iostream>
#include <sstream>
#include <set>
#include <iterator>
#include <string>
#include <vector> 
#include "dependencyGraph.h"


using namespace std;
int main()
{
    {
       cout<< "beginning Empty Test" << endl;
       DependencyGraph *d = new DependencyGraph();
       cout<< "size should be zero " << d->getSize() <<endl; 
    }

    {
       cout<< "checking Dependents with zero size" << endl;
       DependencyGraph *d = new DependencyGraph();

       cout<< "answer is false: actual:  " << d->hasDependents("S") <<endl; 
    }

    {
            
       cout<< "checking Dependees with zero size" << endl;
       DependencyGraph *d = new DependencyGraph();

       cout<< "answer is false: actual:  " << d->hasDependees("S") <<endl; 
    }

    {
            
       cout<< "checking size of links" << endl;
       DependencyGraph *d = new DependencyGraph();
       d->addDependency("x","b");
       d->addDependency("x","c");
       d->addDependency("x","d");



       cout<< "answer is true: actual:  " << d->hasDependents("x") <<endl; 
       cout<< "answer is false: actual:  " << d->hasDependents("b") <<endl; 
       cout<< "answer is false: actual:  " << d->hasDependents("c") <<endl; 
       cout<< "answer is false: actual:  " << d->hasDependents("d") <<endl; 
       cout<< "answer is 3: actual:  " << d->getSize() <<endl; 

    }

    {
        cout<< "simple empty remove test " << endl;
         DependencyGraph *d = new DependencyGraph();
         d->addDependency("x","y");
         cout<< "actual size: 1 " << d->getSize() << endl;
         d->removeDependency("x","y");
         cout<< "bug test, need to decrement size in the remove method" << endl;
         cout<< "actual size: 0 " << d->getSize() << endl;
    }

    {
        cout<< "ienumerable test with dependees" << endl;
        DependencyGraph *d = new DependencyGraph();
        d->addDependency("a","b");
        d->addDependency("d","b");
        vector<string> * dependees = d->getDependees("b");
        cout<< "actual: a, d" << endl;
        cout<< dependees->at(0) << dependees->at(1) << endl;
        

    }

    {
        cout<< "ienumerable test with dependents" << endl;
        DependencyGraph *d = new DependencyGraph();
        d->addDependency("a","b");
        d->addDependency("a","c");
        vector<string> * dependents = d->getDependents("a");
        cout<< "actual: b, c" << endl;
        cout<< dependents->at(0) << dependents->at(1) << endl;
        

    }


    {

        cout<< "Simple Empty Remove Test 2" << endl;
        DependencyGraph *d = new DependencyGraph();
        d->addDependency("x","y");
        vector<string>  *  dependents = d->getDependents("x");
        cout<< "actual: y" << endl;
        cout<< dependents->at(0) << endl;

        vector<string>  *dependees = d->getDependees("y");
        cout<< "actual: x" << endl;
        cout<< dependees->at(0) << endl;

        d->removeDependency("x","y");

        cout<< "actual count is  0: " << d->getSize() << endl;
    }

    {

        cout<< "Test complex replace " << endl;
        DependencyGraph *d = new DependencyGraph();
        d->addDependency("x","b");
        d->addDependency("a","z");
        vector<string> emptyVector; 
        d->replaceDependents("b",emptyVector);
        d->addDependency("y","b");
        vector<string> Vector1;
        Vector1.push_back("c");
        d->replaceDependents("a",Vector1);
        d->addDependency("w","d");
        vector<string> Vector2;
        Vector2.push_back("a");
        Vector2.push_back("c");
        d->replaceDependees("b",Vector2);
        vector<string> Vector3;
        Vector3.push_back("b");
        d->replaceDependees("d",Vector3);
        cout<<"actual size: 4  " << d->getSize() << endl;

    }

    {
        cout<< "Test complex replace2 " << endl;
        DependencyGraph *d = new DependencyGraph();
        d->addDependency("x","b");
        d->addDependency("a","z");
        vector<string> emptyVector; 
        d->replaceDependents("b",emptyVector);
        d->addDependency("y","b");
        vector<string> Vector1;
        Vector1.push_back("c");
        d->replaceDependents("a",Vector1);
        d->addDependency("w","d");
        vector<string> Vector2;
        Vector2.push_back("a");
        Vector2.push_back("c");
        d->replaceDependees("b",Vector2);
        vector<string> Vector3;
        Vector3.push_back("b");
        d->replaceDependees("d",Vector3);
        cout<< "actual: true " << d->hasDependents("a") << endl;
        cout<< "actual: false " << d->hasDependees("a") << endl;
        cout<< "actual: true " << d->hasDependents("b") << endl;
        cout<< "actual: true " << d->hasDependees("b") << endl;

        


    }

        cout<< "Test check Replace contents:  " << endl;
        DependencyGraph *d = new DependencyGraph();
        d->addDependency("x","b");
        d->addDependency("a","z");
        vector<string> emptyVector; 
        d->replaceDependents("b",emptyVector);
        d->addDependency("y","b");
        vector<string> Vector1;
        Vector1.push_back("c");
        d->replaceDependents("a",Vector1);
        d->addDependency("w","d");
        vector<string> Vector2;
        Vector2.push_back("a");
        Vector2.push_back("c");
        d->replaceDependees("b",Vector2);
        vector<string> Vector3;
        Vector3.push_back("b");
        d->replaceDependees("d",Vector3);

        

        cout << "actual: false : " << d->hasDependees("y") << endl;
        vector<string> * dependeesofb = d->getDependees("b");
        cout<< "actual: a, c "  << dependeesofb->at(0) << dependeesofb->at(1) << dependeesofb->size()<< endl;
        vector<string> * dependeesofc = d->getDependees("c");
        cout<< "actual: a "  << dependeesofc->at(0) << dependeesofc->size()<< endl;
        vector<string> * dependeesofd = d->getDependees("d");
        cout<< "actual: b "  << dependeesofd->at(0) << dependeesofd->size()<< endl;





        
    {



    }














    return 0;
}