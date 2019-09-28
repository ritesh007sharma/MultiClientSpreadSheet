#include <iostream>
#include <string>
#include <algorithm>
#include "dependencyGraph.h"

using namespace std;

bool asrtTrue(string msg, bool condition){
    if(condition){
        cout<<" Pass: "<<msg<<endl;
        return true;
    }
    cout<<"Fail: "<<msg<<endl;
}

bool constructorDestructorTest(){
    cout<<"\n Testing Constructor destructor\n";
    {
        DependencyGraph d;
    }
    DependencyGraph *d=new DependencyGraph();
    delete d;

    cout<<" Testing Constructor destructor complete\n";
    return true;
}

bool addTest(){
    cout<<"\n Testing adding\n";
    DependencyGraph d;
    string dependee="B2", dependent="A1";
    d.addDependency(dependee, dependent);
    if(!asrtTrue("size updates", d.getSize()==1))
        return false;
    if(!asrtTrue("has dependee", d.hasDependees(dependent)))
        return false;
    if(!asrtTrue("has dependent", d.hasDependents(dependee)))
        return false;
    //add it again, make sure nothing changes
    d.addDependency(dependee, dependent);
    if(!asrtTrue("size stayed constant", d.getSize()==1))
        return false;

    return true;
}
bool removeTest(){
    cout<<"\n Testing removing\n";
    DependencyGraph d;
    string dependee="B2", dependent="A1";

    //add a dependent
    d.addDependency(dependee, dependent);
    if(!asrtTrue("size updates to 1", d.getSize()==1))
        return false;

    //remove a dependent
    d.RemoveDependency(dependee, dependent);
    if(!asrtTrue("size updates to 0", d.getSize()==0))
        return false;
    if(!asrtTrue("doesn't have dependee", !d.hasDependees(dependent)))
        return false;
    if(!asrtTrue("doesn't have dependent", !d.hasDependents(dependee)))
        return false;

    return true;
}

bool mappingTest(){
    bool ret=true;
    cout<<"\n Testing mapping\n";
    DependencyGraph d;
    string dependee="B2", dependent="A1";
    d.addDependency(dependee, dependent);
    vector<string> *v=d.getDependees(dependent);
    if(!asrtTrue("mapped dependee is correct", v->front()==dependee))
       ret=false;
    delete v;

    v=d.getDependents(dependee);
    if(!asrtTrue("mapped dependent is correct", v->front()==dependent))
        ret=false;
    delete v;
    return ret;
}

bool emptyAndBackTest(){
    cout<<"\n Testing adding multiple dependencies\n";
    DependencyGraph d;
    string dependee="B2", dependent="A1";
    string dependee2="B3", dependent2="A2";

    //simple add and remove
    d.addDependency(dependee, dependent);
    if(!asrtTrue("first size updates to 1", d.getSize()==1))
        return false;

    d.RemoveDependency(dependee, dependent);
    if(!asrtTrue("first size updates to 0", d.getSize()==0))
        return false;

    //add the same element twice then remove
    d.addDependency(dependee, dependent);
    if(!asrtTrue("second size updates to 1", d.getSize()==1))
        return false;
    d.addDependency(dependee, dependent);
    if(!asrtTrue("second size stays at 1", d.getSize()==1))
        return false;

    d.RemoveDependency(dependee, dependent);
    if(!asrtTrue("second size updates to 0", d.getSize()==0))
        return false;


    //add two elements, then remove then both
    d.addDependency(dependee, dependent);
    if(!asrtTrue("third size updates to 1", d.getSize()==1))
        return false;
    d.addDependency(dependee2, dependent2);
    if(!asrtTrue("third size updates to 2", d.getSize()==2))
        return false;

    //remove a dependency that doesn't exist
    d.RemoveDependency(dependee, dependent2);
    if(!asrtTrue("third size stays at 2", d.getSize()==2))
        return false;

    d.RemoveDependency(dependee, dependent);
    if(!asrtTrue("third size updates to 1", d.getSize()==1))
        return false;
    d.RemoveDependency(dependee2, dependent2);
    if(!asrtTrue("third size updates to 0", d.getSize()==0))
        return false;

    if(!asrtTrue("doesn't have dependee", !d.hasDependees(dependent)))
        return false;
    if(!asrtTrue("doesn't have dependent", !d.hasDependents(dependee)))
        return false;
    if(!asrtTrue("doesn't have dependee2", !d.hasDependees(dependent2)))
        return false;
    if(!asrtTrue("doesn't have dependent2", !d.hasDependents(dependee2)))
        return false;

    return true;
}


bool multipleMappingTest(){
    bool ret=true;
    cout<<"\n Testing mapping of multiple items\n";
    DependencyGraph d;
    string dependee="B2", dependent="A1";
    string dependee2="B3", dependent2="A2";


    //add one item
    d.addDependency(dependee, dependent);
    vector<string> *v=d.getDependees(dependent);
    if(!asrtTrue("size is 1", 1==v->size()))
        ret=false;
    if(!asrtTrue("mapped dependee is correct",
                 find(v->begin(), v->end(), dependee) != v->end()))
       ret=false;
    delete v;

    //add another item and see if dependents are at 2
    d.addDependency(dependee2, dependent);
    v=d.getDependees(dependent);
    if(!asrtTrue("size is 2", 2==v->size()))
        ret=false;
    if(!asrtTrue("mapped dependee2 is correct",
                 find(v->begin(), v->end(), dependee2) != v->end()))
       ret=false;
    if(!asrtTrue("mapped dependee is still correct",
                 find(v->begin(), v->end(), dependee) != v->end()))
       ret=false;
    delete v;

    //make sure the dependents are still at 1
    v=d.getDependents(dependee);
    if(!asrtTrue("dependent size is 1", 1==v->size()))
        ret=false;
    if(!asrtTrue("mapped dependent is correct",
                 find(v->begin(), v->end(), dependent) != v->end()))
        ret=false;
    delete v;


    //remove a dependency
    d.RemoveDependency(dependee2, dependent);
    if(!asrtTrue("graph size updates to 1", d.getSize()==1))
        return false;

    v=d.getDependees(dependent);
    if(!asrtTrue("dependee size is now 1", 1==v->size()))
        ret=false;
    //cout<<"dependent size "<<v->size()<<endl;
    //cout<<v->front()<<endl;
    //cout<<v->back()<<endl;
    if(!asrtTrue("mapped dependee is still present",
                 find(v->begin(), v->end(), dependee) != v->end()))
       ret=false;
    delete v;



    //add another item (dependent this time) and see if dependents are at 2
    d.addDependency(dependee, dependent2);
    v=d.getDependents(dependee);
    if(!asrtTrue("size of Dependents is 2", 2==v->size()))
        ret=false;
    if(!asrtTrue("mapped dependent2 is correct",
                 find(v->begin(), v->end(), dependent2) != v->end()))
       ret=false;
    if(!asrtTrue("mapped dependent is still correct",
                 find(v->begin(), v->end(), dependent) != v->end()))
       ret=false;
    delete v;

    //remove a dependency
    d.RemoveDependency(dependee, dependent2);
    if(!asrtTrue("graph size updates to 1", d.getSize()==1))
        return false;

    v=d.getDependents(dependee);
    if(!asrtTrue("dependent size is now 1", 1==v->size()))
        ret=false;
    if(!asrtTrue("mapped dependent is still present",
                 find(v->begin(), v->end(), dependent) != v->end()))
       ret=false;
    delete v;

    return ret;
}

bool replaceDependeesTest(){
    bool ret=true;
    cout<<"\n Testing replacing of dependees\n";
    DependencyGraph d;
    string dependee="B2", dependent="A1", dependent2="A2";
    vector<string> dependees;
    dependees.push_back("B3");
    dependees.push_back("B4");
    dependees.push_back("B5");
    //vector<string> newDependees(dependees);

    //add the initial item
    d.addDependency(dependee, dependent);
    vector<string> *v=d.getDependees(dependent);
    if(!asrtTrue("size is 1", 1==v->size()))
        ret=false;
    if(!asrtTrue("mapped dependee is correct",
                 find(v->begin(), v->end(), dependee) != v->end()))
       ret=false;
    delete v;


    //replace the initial item with the list of 3 dependees
    d.ReplaceDependees(dependent, dependees);

    v=d.getDependees(dependent);
    if(!asrtTrue("size is 3", 3==v->size()))
        ret=false;
    if(!asrtTrue("graph size is 3", 3==d.getSize()))
        ret=false;
    if(!asrtTrue("former dependee is gone",
                 find(v->begin(), v->end(), dependee) == v->end()))
       ret=false;
    if(!asrtTrue("mapped dependee is correct",
                 find(v->begin(), v->end(), dependees[0]) != v->end()))
       ret=false;
    if(!asrtTrue("mapped dependee is correct",
                 find(v->begin(), v->end(), dependees[1]) != v->end()))
       ret=false;
    if(!asrtTrue("mapped dependee is correct",
                 find(v->begin(), v->end(), dependees[2]) != v->end()))
       ret=false;
    delete v;



    //replace the list of 3 items with the same list
    d.ReplaceDependees(dependent, dependees);

    v=d.getDependees(dependent);
    if(!asrtTrue("size is still 3", 3==v->size()))
        ret=false;
    if(!asrtTrue("graph size is 3", 3==d.getSize()))
        ret=false;
    if(!asrtTrue("mapped dependee is still correct",
                 find(v->begin(), v->end(), dependees[0]) != v->end()))
       ret=false;
    if(!asrtTrue("mapped dependee is still correct",
                 find(v->begin(), v->end(), dependees[1]) != v->end()))
       ret=false;
    if(!asrtTrue("mapped dependee is still correct",
                 find(v->begin(), v->end(), dependees[2]) != v->end()))
       ret=false;
    delete v;


    //replace the list of 3 items with 1 item
    vector<string> oneDependee;
    oneDependee.push_back(dependee);
    d.ReplaceDependees(dependent, oneDependee);

    v=d.getDependees(dependent);
    if(!asrtTrue("size is now 1", 1==v->size()))
        ret=false;
    if(!asrtTrue("graph size is now 1", 1==d.getSize()))
        ret=false;
    if(!asrtTrue("mapped one dependee is correct",
                 find(v->begin(), v->end(), oneDependee[0]) != v->end()))
       ret=false;
    if(!asrtTrue("mapped dependee is gone",
                 find(v->begin(), v->end(), dependees[0]) == v->end()))
       ret=false;
    if(!asrtTrue("mapped dependee is gone",
                 find(v->begin(), v->end(), dependees[1]) == v->end()))
       ret=false;
    if(!asrtTrue("mapped dependee is gone",
                 find(v->begin(), v->end(), dependees[2]) == v->end()))
       ret=false;
    delete v;


    //replace an empty dependent with a list
    d.ReplaceDependees(dependent2, dependees);

    v=d.getDependees(dependent2);
    if(!asrtTrue("size is 3", 3==v->size()))
        ret=false;
    if(!asrtTrue("graph size is 4", 4==d.getSize()))
        ret=false;
    if(!asrtTrue("mapped dependee is correct",
                 find(v->begin(), v->end(), dependees[0]) != v->end()))
       ret=false;
    if(!asrtTrue("mapped dependee is correct",
                 find(v->begin(), v->end(), dependees[1]) != v->end()))
       ret=false;
    if(!asrtTrue("mapped dependee is correct",
                 find(v->begin(), v->end(), dependees[2]) != v->end()))
       ret=false;
    delete v;

    //make sure the previous dependent is still intact
    v=d.getDependees(dependent);
    if(!asrtTrue("size is now 1", 1==v->size()))
        ret=false;
    if(!asrtTrue("graph size is still 4", 4==d.getSize()))
        ret=false;
    if(!asrtTrue("mapped one dependee is correct",
                 find(v->begin(), v->end(), oneDependee[0]) != v->end()))
       ret=false;
    delete v;

    return ret;
}


bool replaceDependentsTest(){
    bool ret=true;
    cout<<"\n Testing replacing of Dependents\n";
    DependencyGraph d;
    string dependent="B2", dependee="A1", dependee2="A2";
    vector<string> dependents;
    dependents.push_back("B3");
    dependents.push_back("B4");
    dependents.push_back("B5");
    //vector<string> newDependees(dependees);

    //add the initial item
    d.addDependency(dependee, dependent);
    vector<string> *v=d.getDependents(dependee);
    if(!asrtTrue("size is 1", 1==v->size()))
        ret=false;
    if(!asrtTrue("mapped dependee is correct",
                 find(v->begin(), v->end(), dependent) != v->end()))
       ret=false;
    delete v;


    //replace the initial item with the list of 3 dependees
    d.ReplaceDependents(dependee, dependents);

    v=d.getDependents(dependee);
    if(!asrtTrue("size is 3", 3==v->size()))
        ret=false;
    if(!asrtTrue("graph size is 3", 3==d.getSize()))
        ret=false;
    if(!asrtTrue("former dependee is gone",
                 find(v->begin(), v->end(), dependent) == v->end()))
       ret=false;
    if(!asrtTrue("mapped dependee is correct",
                 find(v->begin(), v->end(), dependents[0]) != v->end()))
       ret=false;
    if(!asrtTrue("mapped dependee is correct",
                 find(v->begin(), v->end(), dependents[1]) != v->end()))
       ret=false;
    if(!asrtTrue("mapped dependee is correct",
                 find(v->begin(), v->end(), dependents[2]) != v->end()))
       ret=false;
    delete v;



    //replace the list of 3 items with the same list
    d.ReplaceDependents(dependee, dependents);

    v=d.getDependents(dependee);
    if(!asrtTrue("size is still 3", 3==v->size()))
        ret=false;
    if(!asrtTrue("graph size is 3", 3==d.getSize()))
        ret=false;
    if(!asrtTrue("mapped dependee is still correct",
                 find(v->begin(), v->end(), dependents[0]) != v->end()))
       ret=false;
    if(!asrtTrue("mapped dependee is still correct",
                 find(v->begin(), v->end(), dependents[1]) != v->end()))
       ret=false;
    if(!asrtTrue("mapped dependee is still correct",
                 find(v->begin(), v->end(), dependents[2]) != v->end()))
       ret=false;
    delete v;


    //replace the list of 3 items with 1 item
    vector<string> oneDependent;
    oneDependent.push_back("C5");
    d.ReplaceDependents(dependee, oneDependent);

    v=d.getDependents(dependee);
    if(!asrtTrue("size is now 1", 1==v->size()))
        ret=false;
    if(!asrtTrue("graph size is now 1", 1==d.getSize()))
        ret=false;
    if(!asrtTrue("mapped one dependee is correct",
                 find(v->begin(), v->end(), oneDependent[0]) != v->end()))
       ret=false;
    if(!asrtTrue("mapped dependee is gone",
                 find(v->begin(), v->end(), dependents[0]) == v->end()))
       ret=false;
    if(!asrtTrue("mapped dependee is gone",
                 find(v->begin(), v->end(), dependents[1]) == v->end()))
       ret=false;
    if(!asrtTrue("mapped dependee is gone",
                 find(v->begin(), v->end(), dependents[2]) == v->end()))
       ret=false;
    delete v;


    //replace an empty dependent with a list
    d.ReplaceDependents(dependee2, dependents);

    v=d.getDependents(dependee2);
    if(!asrtTrue("size is 3", 3==v->size()))
        ret=false;
    if(!asrtTrue("graph size is 4", 4==d.getSize()))
        ret=false;
    if(!asrtTrue("mapped dependee is correct",
                 find(v->begin(), v->end(), dependents[0]) != v->end()))
       ret=false;
    if(!asrtTrue("mapped dependee is correct",
                 find(v->begin(), v->end(), dependents[1]) != v->end()))
       ret=false;
    if(!asrtTrue("mapped dependee is correct",
                 find(v->begin(), v->end(), dependents[2]) != v->end()))
       ret=false;
    delete v;

    //make sure the previous dependent is still intact
    v=d.getDependents(dependee);
    if(!asrtTrue("size is now 1", 1==v->size()))
        ret=false;
    if(!asrtTrue("graph size is still 4", 4==d.getSize()))
        ret=false;
    if(!asrtTrue("mapped one dependee is correct",
                 find(v->begin(), v->end(), oneDependent[0]) != v->end()))
       ret=false;
    delete v;

    return ret;
}


int main(){
    bool success=true;
    cout<<"Testing Started\n";
    if(!constructorDestructorTest()) success=false;
    if(!addTest()) success=false;
    if(!removeTest()) success=false;
    if(!mappingTest()) success=false;
    if(!emptyAndBackTest()) success=false;
    if(!multipleMappingTest()) success=false;
    if(!replaceDependeesTest()) success=false;
    if(!replaceDependentsTest()) success=false;

    if(success)cout<<"\n  All Tests Passed\n\n";
    else cout<<"\n  Test Failed!\n\n";
}
