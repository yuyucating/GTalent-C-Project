namespace InventorySystem.Models;

public abstract class Animal
{
    public String Name { get; set; }

    public Animal()
    {
    }

    public Animal(String name)
    {
        Name = name;
    }

    public abstract void MakeSound();
}