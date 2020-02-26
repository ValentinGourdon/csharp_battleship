public class Boat {
    protected int id;
    protected string name;
    protected int lenght;
    protected int touched;
    // position : true = horizontal, false = vertical
    protected bool position;

    public Boat() {}

    public override bool Equals(object obj) {
        if(obj == null)
            return false;
        Boat b = obj as Boat;
        if(b == null)
            return false;
        return this.id == b.id && this.name == b.name && this.lenght == b.lenght
                    && this.touched == b.touched && this.position == b.position;
    }

    public override int GetHashCode() {
        return this.id.GetHashCode() * this.name.GetHashCode() * this.lenght.GetHashCode()
                    * this.touched.GetHashCode() * this.position.GetHashCode();
    }

    public override string ToString() {
        string ret = this.id + " - Name : " + this.name;
        ret += " - Lenght : " + this.lenght;
        return ret;
    }

    public string ToStringFull() {
        string ret = this.id + " - Name : " + this.name;
        ret += " - Lenght : " + this.lenght + " - Touched : " + this.touched;
        if(position)
            ret += " - Position : Horizontal";
        else
            ret += " - Position : Vertical";
        return ret;
    }

    public int getId() {
        return this.id;
    }

    public string getName()
    {
        return this.name;
    }

    public int getLenght()
    {
        return this.lenght;
    }

    public int getTouched()
    {
        return this.touched;
    }

    public void setTouched()
    {
        this.touched++;
    }

    public bool getPosition()
    {
        return this.position;
    }

    public void setPosition(bool position) {
        this.position = position;
    }

    public int getById(int id) {
        if(this.id == id)
            return this.id;
        else
            return 0;
    }

    public bool isAlive() {
        if(this.lenght > this.touched)
            return true;
        else
            return false;
    }
}