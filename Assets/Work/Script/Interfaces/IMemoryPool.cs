public interface IMemoryPool
{
    public string Key { get; set; }
    public void Release();
}