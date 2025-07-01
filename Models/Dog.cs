namespace InventorySystem.Models;

public class Dog : Animal
{
    public Dog()
    {
    }
    public Dog(string name) : base(name)
    {
    }

    public override void MakeSound()
    {
        Console.WriteLine($"{Name} is barking!!");
    }
}